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

            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(string url, IEnumerable<KeyValuePair<string, string>> body = null)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };

            var content = new FormUrlEncodedContent(body ?? Enumerable.Empty<KeyValuePair<string, string>>());

            var response = await client.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
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

            var response = await client.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }

        // Currently this method used /api/v1/statuses/:id only, and return empty
        public async Task<string> Delete(string url)
        {
            var client = new HttpClient(new AuthHttpClientHandler(AccessToken))
            {
                BaseAddress = Host
            };

            var response = await client.DeleteAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<T> Get<T>(string url)
            where T : class
        {
            var response = await Get(url);
            return MastodonJsonConverter.TryParse<T>(response);
        }

        public async Task<T> Post<T>(string url, IEnumerable<KeyValuePair<string, string>> body = null)
            where T : class
        {
            var response = await Post(url, body);
            return MastodonJsonConverter.TryParse<T>(response);
        }

        public async Task<T> Patch<T>(string url, IEnumerable<KeyValuePair<string, string>> body = null)
            where T : class
        {
            var response = await Patch(url, body);
            return MastodonJsonConverter.TryParse<T>(response);
        }

        public async Task<T> Delete<T>(string url)
            where T : class
        {
            var response = await Get(url);
            return MastodonJsonConverter.TryParse<T>(response);
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

                            if (string.IsNullOrEmpty(s) || s.StartsWith(":")) {
                                continue;
                            }

                            if (s.StartsWith("event: ")) {
                                eventSubject.OnNext(StreamEventExtentions.FromString(s.Substring("event: ".Length)));
                            }
                            else if (s.StartsWith("data: ")) {
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
                    return MastodonJsonConverter.TryParse<Status>(entityBody);
                case StreamEvent.Notification:
                    return MastodonJsonConverter.TryParse<Notification>(entityBody);
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
