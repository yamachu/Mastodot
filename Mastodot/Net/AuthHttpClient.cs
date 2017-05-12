using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Mastodot.Utils;
using Mastodot.Entities;
using Mastodot.Enums;

namespace Mastodot.Net
{
    internal class AuthHttpClient
    {
        private string AccessToken;
        private Uri Host;

        public AuthHttpClient(string accessToken, string host)
        {
            AccessToken = accessToken;
            Host = new Uri($"https://{host}");
        }

        public async Task<string> Get(string url)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };

            var response = await client.GetAsync(url).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> GetEntirely(string url)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };

            return await client.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<string> Post(string url, IEnumerable<KeyValuePair<string, string>> body = null)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };

            var content = new FormUrlEncodedContent(body ?? Enumerable.Empty<KeyValuePair<string, string>>());

            var response = await client.PostAsync(url, content).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
#if !NETSTANDARD1_1
        public async Task<string> PostWithMedia(string url, string filePath)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(File.OpenRead(filePath)), "file", "file");

            var response = await client.PostAsync(url, content).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
#endif
        public async Task<string> PostWithMedia(string url, byte[] image)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };

            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(image), "file", "file");

            var response = await client.PostAsync(url, content).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<string> Patch(string url, IEnumerable<KeyValuePair<string, string>> body = null)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };
            var httpMethod = new HttpMethod("PATCH");
            var message = new HttpRequestMessage(httpMethod, url);

            var content = new FormUrlEncodedContent(body ?? Enumerable.Empty<KeyValuePair<string, string>>());
            message.Content = content;

            var response = await client.SendAsync(message).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        // Currently this method used /api/v1/statuses/:id only, and return empty
        public async Task<string> Delete(string url)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };

            var response = await client.DeleteAsync(url).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<T> Get<T>(string url)
            where T : class
        {
            var response = await Get(url).ConfigureAwait(false);
            return MastodonJsonConverter.TryDeserialize<T>(response);
        }

        public async Task<ResponseCollection<T>> GetCollection<T>(string url)
            where T : IBaseMastodonEntity
        {
            var response = await GetEntirely(url).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var entity = MastodonJsonConverter.TryDeserialize<IEnumerable<T>>(content);
            var result = new ResponseCollection<T>(entity);

            result.RawJson = content;

            IEnumerable<string> link;
            if (response.Headers.TryGetValues("Link", out link))
            {
                result.Links = LinkHeaderParser.GetHeader(link.First());
            }

            return result;
        }

        public async Task<T> Post<T>(string url, IEnumerable<KeyValuePair<string, string>> body = null)
            where T : class
        {
            var response = await Post(url, body).ConfigureAwait(false);
            return MastodonJsonConverter.TryDeserialize<T>(response);
        }
#if !NETSTANDARD1_1
        public async Task<T> PostWithMedia<T>(string url, string fileName)
            where T : class
        {
            var response = await PostWithMedia(url, fileName).ConfigureAwait(false);
            return MastodonJsonConverter.TryDeserialize<T>(response);
        }
#endif
        public async Task<T> PostWithMedia<T>(string url, byte[] image)
            where T : class
        {
            var response = await PostWithMedia(url, image).ConfigureAwait(false);
            return MastodonJsonConverter.TryDeserialize<T>(response);
        }

        public async Task<T> Patch<T>(string url, IEnumerable<KeyValuePair<string, string>> body = null)
            where T : class
        {
            var response = await Patch(url, body).ConfigureAwait(false);
            return MastodonJsonConverter.TryDeserialize<T>(response);
        }

        public async Task<T> Delete<T>(string url)
            where T : class
        {
            var response = await Delete(url).ConfigureAwait(false);
            return MastodonJsonConverter.TryDeserialize<T>(response);
        }

        // http://neue.cc/2013/02/27_398.html
        public IObservable<IStreamEntity> GetObservable(string url)
        {
            return Observable.Create<IStreamEntity>(async (observer, ct) =>
            {
                var eventSubject = new Subject<StreamEvent>();
                var entityBodySubject = new Subject<string>();

                eventSubject.Zip(entityBodySubject, (e, b) => ConvertStreamEntity(e, b))
                            .Subscribe(x => observer.OnNext(x));

                try
                {
                    var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
                    {
                        BaseAddress = Host
                    };
                    client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;

                    using (var stream = await client.GetStreamAsync(url))
                    using (var sr = new StreamReader(stream))
                    {
                        while (!sr.EndOfStream && !ct.IsCancellationRequested)
                        {
                            var s = await sr.ReadLineAsync();

                            if (string.IsNullOrEmpty(s) || s.StartsWith(":"))
                            {
                                continue;
                            }

                            if (s.StartsWith("event: "))
                            {
                                eventSubject.OnNext(StreamEventExtentions.FromString(s.Substring("event: ".Length)));
                            }
                            else if (s.StartsWith("data: "))
                            {
                                entityBodySubject.OnNext(s.Substring("data: ".Length));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    eventSubject.OnError(ex);
                    entityBodySubject.OnError(ex);
                    return;
                }
                if (!ct.IsCancellationRequested)
                {
                    observer.OnCompleted();
                    eventSubject.OnCompleted();
                    entityBodySubject.OnCompleted();
                }
            });
        }

        private IStreamEntity ConvertStreamEntity(StreamEvent ev, string entityBody)
        {
            switch (ev)
            {
                case StreamEvent.Update:
                    return MastodonJsonConverter.TryDeserialize<Status>(entityBody);
                case StreamEvent.Notification:
                    return MastodonJsonConverter.TryDeserialize<Notification>(entityBody);
                case StreamEvent.Delete:
                    return new DeletedStream
                    {
                        StatusId = int.Parse(entityBody)
                    };
                default:
                    throw new Exception();
            }
        }
    }
}
