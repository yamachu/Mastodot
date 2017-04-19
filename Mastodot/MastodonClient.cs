using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Mastodot.Consts;
using Mastodot.Entities;
using Mastodot.Net;
using Newtonsoft.Json;

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

        public Task<Account> GetAccount(int id)
        {
            return GetClient().Get<Account>(string.Format(ApiMethods.GetAccount, id));
        }

        public Task<Account> GetCurrentAccount()
        {
            return GetClient().Get<Account>(ApiMethods.GetCurrentAccount);
        }

        public Task<Account> UpdateCurrentAccount(string displayName = null, string note = null, string avatar = null, string header = null)
        {
            var body = new Dictionary<string, string>{
                {"display_name", displayName},
                {"note", note},
                {"avatar", avatar},
                {"header", header}
            }.Where(x => x.Value != null);

            return GetClient().Patch<Account>(ApiMethods.UpdateCurrentAccount, body);
        }

        public Task<IEnumerable<Account>> GetFollowers(int id)
        {
            return GetClient().Get<IEnumerable<Account>>(string.Format(ApiMethods.GetFollowers, id));
        }

        public Task<IEnumerable<Account>> GetFollowing(int id)
        {
            return GetClient().Get<IEnumerable<Account>>(string.Format(ApiMethods.GetFollowing, id));
        }

        public Task<IEnumerable<Status>> GetStatuses(int id, bool? onlyMedia = null, bool? excludeReplies = null)
        {
            var query = new Dictionary<string, object>{
                {"only_media", onlyMedia},
                {"exclude_replies", excludeReplies}
            }.Where(x => x.Value != null)
             .Select(x => x.ToUrlFormattedString())
             .Aggregate((x, y) => $"{x}&{y}");

            return GetClient().Get<IEnumerable<Status>>(string.Format(ApiMethods.GetStatuses, id, query.Length != 0 ? $"&{query}" : ""));
        }

        public Task<Relationship> Follow(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Follow));
        }

        public Task<Relationship> Unfollow(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unfollow));
        }

        public Task<Relationship> Block(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Block));
        }

        public Task<Relationship> Unblock(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unblock));
        }

        public Task<Relationship> Mute(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Mute));
        }

        public Task<Relationship> Unmute(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unmute));
        }

        public Task<IEnumerable<Relationship>> GetRelationships(IEnumerable<int> ids)
        {
            var query = new Dictionary<string, object>
            {
                {"id", ids}
            }.Select(x => x.ToUrlFormattedString())
             .Aggregate((x, y) => $"{x}&{y}");

            return GetClient().Get<IEnumerable<Relationship>>($"{ApiMethods.GetRelationships}{(query.Length != 0 ? "?" + query : "")}");
        }

        public Task<IEnumerable<Account>> SearchAccount(string searchQuery, int limit = 40)
        {
            var query = new Dictionary<string, object>
            {
                {"q", searchQuery},
                {"limit", limit}
            }.Select(x => x.ToUrlFormattedString())
             .Aggregate((x, y) => $"{x}&{y}");

            return GetClient().Get<IEnumerable<Account>>($"{ApiMethods.SearchForAccounts}{(query.Length != 0 ? "?" + query : "")}");
        }

        public Task<IEnumerable<Account>> GetBlockedUsers()
        {
            return GetClient().Get<IEnumerable<Account>>(ApiMethods.GetBlocks);
        }

        public Task<IEnumerable<Status>> GetFavourites()
        {
            return GetClient().Get<IEnumerable<Status>>(ApiMethods.GetFavourites);
        }

        public Task<IEnumerable<Account>> GetFollowRequests()
        {
            return GetClient().Get<IEnumerable<Account>>(ApiMethods.GetFollowRequests);
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

        public Task<Attachment> UploadMedia(string encodedFile)
        {
            var param = new Dictionary<string, object>
            {
                {"file", encodedFile}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

            return GetClient().Post<Attachment>(ApiMethods.UploadMedia, param);
        }

        public Task<IEnumerable<Account>> GetMutes()
        {
            return GetClient().Get<IEnumerable<Account>>(ApiMethods.GetMutes);
        }

        public Task<IEnumerable<Notification>> GetNotifications()
        {
            return GetClient().Get<IEnumerable<Notification>>(ApiMethods.GetNotifications);
        }

        public Task<Notification> GetSingleNotifications()
        {
            return GetClient().Get<Notification>(ApiMethods.GetSingleNotifications);
        }

        public Task ClearNotifications()
        {
            return GetClient().Post(ApiMethods.ClearNotifications);
        }

        public Task<IEnumerable<Report>> GetReports()
        {
            return GetClient().Get<IEnumerable<Report>>(ApiMethods.GetReports);
        }

        public Task<Report> ReportUser(int accountId, IEnumerable<int> statusIds, string comment)
        {
            var param = new Dictionary<string, object>
            {
                {"accountId", accountId},
                {"status_ids", JsonConvert.SerializeObject(statusIds)},
                {"comment", comment}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

            return GetClient().Post<Report>(ApiMethods.ReportUser, param);
        }

        public Task<Results> Search(string searchQuery, bool searchGlobal = false)
        {
            var query = new Dictionary<string, object>
            {
                {"q", searchQuery},
                {"resolve", searchGlobal},
            }.Select(x => x.ToUrlFormattedString())
             .Aggregate((x, y) => $"{x}&{y}");

            return GetClient().Get<Results>($"{ApiMethods.Search}{(query.Length != 0 ? "?" + query : "")}");
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

        public Task<IEnumerable<Account>> GetStatusRebloggedAccounts(int id)
        {
            return GetClient().Get<IEnumerable<Account>>(string.Format(ApiMethods.GetStatusRebloggedBy, id));
        }

        public Task<IEnumerable<Account>> GetStatusFavouritedAccounts(int id)
        {
            return GetClient().Get<IEnumerable<Account>>(string.Format(ApiMethods.GetStatusFavouritedBy, id));
        }

        public Task<Status> PostNewStatus(string status, int? inReplyToId = null, IEnumerable<int> mediaIds = null, bool? sensitive = null, string spoilerText = null, Enums.Visibility visibility = Enums.Visibility.Public)
        {
            var param = new Dictionary<string, object>
            {
                {"status", status},
                {"in_reply_to_id", inReplyToId},
                {"media_ids", mediaIds != null ? (object)JsonConvert.SerializeObject(mediaIds): mediaIds},
                {"sensitive", sensitive},
                {"spoiler_text", spoilerText},
                {"visibility", visibility.ToString().ToLower()}
            }.Where(x => x.Value != null)
             .Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

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

        public Task<IEnumerable<Status>> GetRecentHomeTimeline()
        {
            return GetClient().Get<IEnumerable<Status>>(ApiMethods.GetHomeTimeline);
        }

        public Task<IEnumerable<Status>> GetRecentPublicTimeline(bool local = true)
        {
            var query = new Dictionary<string, object>
            {
                {"local", local}
            }.Select(x => x.ToUrlFormattedString())
             .Aggregate((x, y) => $"{x}&{y}");

            return GetClient().Get<IEnumerable<Status>>($"{ApiMethods.GetPublicTimeline}?{query}");
        }

        public Task<IEnumerable<Status>> GetRecentHomeTimeline(string hashtag, bool local = true)
        {
            var query = new Dictionary<string, object>
            {
                {"hashtag", hashtag},
                {"local", local}
            }.Select(x => x.ToUrlFormattedString())
             .Aggregate((x, y) => $"{x}&{y}");

            return GetClient().Get<IEnumerable<Status>>($"{ApiMethods.GetHastagTimeline}?{query}");
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
            }.Select(x => x.ToUrlFormattedString())
             .Aggregate((x, y)=> $"{x}&{y}");
            
            return GetClient(host).GetObservable($"{ApiMethods.GetPublicStream}{(query.Length != 0 ? "?" + query: "")}");
        }
    }

    internal static class ParameterFormatUtilExtentions
    {
        public static string ToUrlFormattedString(this KeyValuePair<string, object> kvp)
        {
            // ToDo: Encoding and type much some types
            // Format ids[]=1&ids[]=2 ...
            return $"{kvp.Key}={kvp.Value}";
        }
    }
}
