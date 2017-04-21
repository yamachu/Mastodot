using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Mastodot.Consts;
using Mastodot.Entities;
using Mastodot.Net;

namespace Mastodot
{
    public class MastodonClient
    {
        public string Host { get; }
        public string AccessToken { get; }

        public MastodonClient(string host, string accessToken)
        {
            Host = host;
            AccessToken = accessToken;
        }

        private AuthHttpClient GetClient(string host = null)
        {
            return new AuthHttpClient(AccessToken, host ?? Host);
        }

        private string FullUrl(string path, string query)
        {
            return $"{path}{(string.IsNullOrWhiteSpace(query) ? "&" + query : "")}";
        }

        public Task<Account> GetAccount(int id)
        {
            return GetClient().Get<Account>(string.Format(ApiMethods.GetAccount, id));
        }

        public Task<Account> GetCurrentAccount()
        {
            return GetClient().Get<Account>(ApiMethods.GetCurrentAccount);
        }

        public Task<Account> UpdateCurrentAccount(string displayName = null
                                                  , string note = null
                                                  , string avatar = null
                                                  , string header = null)
        {
            var body = new Dictionary<string, string>
            {
                {"display_name", displayName},
                {"note", note},
                {"avatar", avatar},
                {"header", header}
            }.Where(x => x.Value != null);

            return GetClient().Patch<Account>(ApiMethods.UpdateCurrentAccount, body);
        }

        public Task<IEnumerable<Account>> GetFollowers(int id
                                                       , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Account>>(FullUrl(string.Format(ApiMethods.GetFollowers, id), query));
        }

        public Task<IEnumerable<Account>> GetFollowing(int id
                                                       , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Account>>(FullUrl(string.Format(ApiMethods.GetFollowing, id), query));
        }

        public Task<IEnumerable<Status>> GetStatuses(int id
                                                     , bool? onlyMedia = default(bool?), bool? excludeReplies = default(bool?)
                                                     , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>
            {
                {"only_media", onlyMedia},
                {"exclude_replies", excludeReplies}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().Get<IEnumerable<Status>>(FullUrl(string.Format(ApiMethods.GetStatuses, id), query));
        }

        public Task<Relationship> Follow(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Follow, id));
        }

        public Task<Relationship> Unfollow(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unfollow, id));
        }

        public Task<Relationship> Block(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Block, id));
        }

        public Task<Relationship> Unblock(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unblock, id));
        }

        public Task<Relationship> Mute(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Mute, id));
        }

        public Task<Relationship> Unmute(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unmute, id));
        }

        public Task<IEnumerable<Relationship>> GetRelationships(IEnumerable<int> ids
                                                               , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>
            {
                {"id", ids}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().Get<IEnumerable<Relationship>>(FullUrl(ApiMethods.GetRelationships, query));
        }

        public Task<IEnumerable<Account>> SearchAccount(string searchQuery
                                                        , int limit = 40
                                                        , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>
            {
                {"q", searchQuery},
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().Get<IEnumerable<Account>>(FullUrl(ApiMethods.SearchForAccounts, query));
        }

        public Task<IEnumerable<Account>> GetBlockedUsers(int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Account>>(FullUrl(ApiMethods.GetBlocks, query));
        }

        public Task<IEnumerable<Status>> GetFavourites(int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Status>>(FullUrl(ApiMethods.GetFavourites, query));
        }

        public Task<IEnumerable<Account>> GetFollowRequests(int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Account>>(FullUrl(ApiMethods.GetFollowRequests, query));
        }

        public Task AuthorizeFollowRequest(int id)
        {
            var param = new Dictionary<string, object>
            {
                {"id", id}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

            return GetClient().Post(string.Format(ApiMethods.AuthorizeFollowRequest, id));
        }

        public Task RejectFollowRequest(int id)
        {
            var param = new Dictionary<string, object>
            {
                {"id", id}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

            return GetClient().Post(string.Format(ApiMethods.RejectFollowRequest, id));
        }

        public Task<Account> RemoteFollow(string fullUserId)
        {
            var param = new Dictionary<string, object>
            {
                {"uri", fullUserId}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

            return GetClient().Post<Account>(ApiMethods.RemoteFollow);
        }

        public Task<Instance> GetInstance()
        {
            // Does not require authentication
            return GetClient().Get<Instance>(ApiMethods.GetInstance);
        }

        public Task<Attachment> UploadMedia(string filePath)
        {
            return GetClient().PostWithMedia<Attachment>(ApiMethods.UploadMedia, filePath);
        }

        public Task<Attachment> UploadMedia(byte[] image)
        {
            return GetClient().PostWithMedia<Attachment>(ApiMethods.UploadMedia, image);
        }

        public Task<IEnumerable<Account>> GetMutes(int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Account>>(FullUrl(ApiMethods.GetMutes, query));
        }

        public Task<IEnumerable<Notification>> GetNotifications(int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Notification>>(FullUrl(ApiMethods.GetNotifications, query));
        }

        public Task<Notification> GetSingleNotifications()
        {
            return GetClient().Get<Notification>(ApiMethods.GetSingleNotifications);
        }

        public Task ClearNotifications()
        {
            return GetClient().Post(ApiMethods.ClearNotifications);
        }

        public Task<IEnumerable<Report>> GetReports(int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Report>>(FullUrl(ApiMethods.GetReports, query));
        }

        public Task<Report> ReportUser(int accountId, IEnumerable<int> statusIds, string comment)
        {
            var param = new Dictionary<string, object>
            {
                {"accountId", accountId},
                {"comment", comment}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()))
            .ToList();
            param.AddIntArrayParameter("status_ids", statusIds);

            return GetClient().Post<Report>(ApiMethods.ReportUser, param);
        }

        public Task<Results> Search(string searchQuery, bool searchGlobal = false)
        {
            var query = new Dictionary<string, object>
            {
                {"q", searchQuery},
                {"resolve", searchGlobal},
            }.ToQueryString();

            return GetClient().Get<Results>(FullUrl(ApiMethods.Search, query));
        }

        public Task<Status> GetStatus(int id)
        {
            return GetClient().Get<Status>(string.Format(ApiMethods.GetStatus, id));
        }

        public Task<Context> GetStausContext(int id)
        {
            return GetClient().Get<Context>(string.Format(ApiMethods.GetStatusContext, id));
        }

        public Task<Card> GetStatusCard(int id)
        {
            return GetClient().Get<Card>(string.Format(ApiMethods.GetStatusCard, id));
        }

        public Task<IEnumerable<Account>> GetStatusRebloggedAccounts(int id
                                                                    , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Account>>(FullUrl(string.Format(ApiMethods.GetStatusRebloggedBy, id), query));
        }

        public Task<IEnumerable<Account>> GetStatusFavouritedAccounts(int id
                                                                     , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Account>>(FullUrl(string.Format(ApiMethods.GetStatusFavouritedBy, id), query));
        }

        public Task<Status> PostNewStatus(string status, int? inReplyToId = null, IEnumerable<int> mediaIds = null, bool? sensitive = default(bool?), string spoilerText = null, Enums.Visibility visibility = Enums.Visibility.Public)
        {
            var param = new Dictionary<string, object>
            {
                {"status", status},
                {"in_reply_to_id", inReplyToId.HasValue? (object)inReplyToId.Value: null},
                {"sensitive", sensitive.HasValue? (object)sensitive.Value: null},
                {"spoiler_text", spoilerText},
                {"visibility", visibility.ToString().ToLower()},
            }.Where(x => x.Value != null)
             .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()))
             .ToList();

            if (mediaIds != null)
            {
                param.AddIntArrayParameter("media_ids", mediaIds);
            }

            return GetClient().Post<Status>(ApiMethods.PostNewStatus, param);
        }

        public Task DeleteStatus(int id)
        {
            return GetClient().Delete(string.Format(ApiMethods.DeleteStatus, id));
        }

        public Task<Status> Reblog(int id)
        {
            return GetClient().Post<Status>(string.Format(ApiMethods.ReblogStatus, id));
        }

        public Task<Status> Unreblog(int id)
        {
            return GetClient().Post<Status>(string.Format(ApiMethods.UnreblogStatus, id));
        }

        public Task<Status> Favourite(int id)
        {
            return GetClient().Post<Status>(string.Format(ApiMethods.FavouritingStatus, id));
        }

        public Task<Status> Unfavourite(int id)
        {
            return GetClient().Post<Status>(string.Format(ApiMethods.UnfavouritingStatus, id));
        }

        public Task<IEnumerable<Status>> GetRecentHomeTimeline(int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>()
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().Get<IEnumerable<Status>>(FullUrl(ApiMethods.GetHomeTimeline, query));
        }

        public Task<IEnumerable<Status>> GetRecentPublicTimeline(bool local = true
                                                                , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>
            {
                {"local", local},
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().Get<IEnumerable<Status>>(FullUrl(ApiMethods.GetPublicTimeline, query));
        }

        public Task<IEnumerable<Status>> GetRecentHomeTimeline(string hashtag
                                                               , bool local = true
                                                               , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>
            {
                {"hashtag", hashtag},
                {"local", local}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().Get<IEnumerable<Status>>(FullUrl(ApiMethods.GetHastagTimeline, query));
        }

        public IObservable<IStreamEntity> GetObservableHomeTimeline(string host = null)
        {
            return GetClient(host).GetObservable(ApiMethods.GetUserStream);
        }

        public IObservable<IStreamEntity> GetObservablePublicTimeline(string host = null)
        {
            return GetClient(host).GetObservable(ApiMethods.GetPublicStream);
        }

        public IObservable<IStreamEntity> GetObservableHashtagTimeline(string hashtag, string host = null)
        {
            var query = new Dictionary<string, object>
            {
                {"tag", hashtag}
            }.Where(x => !string.IsNullOrWhiteSpace(x.Value.ToString()))
             .Select(x => x.ToUrlFormattedQueryString())
             .Aggregate((x, y) => $"{x}&{y}");

            return GetClient(host).GetObservable(FullUrl(ApiMethods.GetPublicStream, query));
        }
    }

    internal static class ParameterFormatUtilExtentions
    {
        public static string ToUrlFormattedQueryString(this KeyValuePair<string, object> kvp)
        {
            if (kvp.Value == null)
            {
                return "";
            }

            if (kvp.Value is IEnumerable<int>)
            {
                return ((IEnumerable<int>)kvp.Value)
                    .Select(i => $"{kvp.Key}[]={i}")
                    .Aggregate((x, y) => $"{x}&{y}");
            }

            return $"{kvp.Key}={System.Net.WebUtility.UrlEncode(kvp.Value.ToString())}";
        }

        public static ICollection<KeyValuePair<string, string>> AddIntArrayParameter(this ICollection<KeyValuePair<string, string>> self, string name, IEnumerable<int> arrayParam)
        {
            foreach (var item in arrayParam)
            {
                self.Add(new KeyValuePair<string, string>($"{name}[]", item.ToString()));
            }

            return self;
        }

        public static Dictionary<string, object> AddRangeParameter(this Dictionary<string, object> dict, int? maxId, int? sinceId)
        {
            if (maxId.HasValue)
            {
                dict.Add("max_id", maxId.Value);
            }
            if (sinceId.HasValue)
            {
                dict.Add("since_id", sinceId.Value);
            }

            return dict;
        }

        public static string ToQueryString(this Dictionary<string, object> dict)
        {
            return dict.Where(x => x.Value != null)
                       .Select(x => x.ToUrlFormattedQueryString())
                       .Aggregate((x, y) => $"{x}&{y}");
        }
    }
}
