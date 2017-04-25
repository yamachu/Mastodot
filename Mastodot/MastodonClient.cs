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
        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <value>Current connected Mastodon host.</value>
        public string Host { get; }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mastodot.MastodonClient"/> class.
        /// </summary>
        /// <param name="host">Mastodon host URL</param>
        /// <param name="accessToken">AccessToken</param>
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
            return $"{path}{(!string.IsNullOrWhiteSpace(query) ? "?" + query : "")}";
        }

        /// <summary>
        /// Fetching an account
        /// </summary>
        /// <returns>The account</returns>
        /// <param name="id">Target AccountID</param>
        public Task<Account> GetAccount(int id)
        {
            return GetClient().Get<Account>(string.Format(ApiMethods.GetAccount, id));
        }

        /// <summary>
        /// Getting the current user
        /// </summary>
        /// <returns>The current account.</returns>
        public Task<Account> GetCurrentAccount()
        {
            return GetClient().Get<Account>(ApiMethods.GetCurrentAccount);
        }

        /// <summary>
        /// Updating the current user
        /// </summary>
        /// <returns>The current authenticated account.</returns>
        /// <param name="displayName"> The name to display in the user's profile</param>
        /// <param name="note"> A new biography for the user</param>
        /// <param name="avatar">Avatar that Base64Encoded image string</param>
        /// <param name="header">Header that Base64Encoded image string</param>
        /// <see cref="Utils.ImageConverter"/>
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

        /// <summary>
        /// Getting an account's followers
        /// </summary>
        /// <returns>The target Id's followers </returns>
        /// <param name="id">Target AccountID</param>
        /// <param name="maxId">Get a list of followers with ID less than or equal this value</param>
        /// <param name="sinceId">Get a list of followers with ID greater than this value</param>
        /// <param name="limit">Maximum number of accounts to get (Default 40, Max 80)</param>
        public Task<ResponseCollection<Account>> GetFollowers(int id
                                                       , int? maxId = default(int?), int? sinceId = default(int?)
                                                       , int limit = 40)
        {
            var query = new Dictionary<string, object>()
            {
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Account>(FullUrl(string.Format(ApiMethods.GetFollowers, id), query));
        }

        /// <summary>
        /// Getting who account is following
        /// </summary>
        /// <returns>The target Id's following.</returns>
        /// <param name="id">Target AccountID</param>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 40, Max 80)</param>
        public Task<ResponseCollection<Account>> GetFollowing(int id
                                                       , int? maxId = default(int?), int? sinceId = default(int?)
                                                       , int limit = 40)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Account>(FullUrl(string.Format(ApiMethods.GetFollowing, id), query));
        }

        /// <summary>
        /// Getting an account's statuses
        /// </summary>
        /// <returns>The statuses.</returns>
        /// <param name="id">Target StatusID</param>
        /// <param name="onlyMedia">Only return statuses that have media attachments</param>
        /// <param name="excludeReplies">Skip statuses that reply to other statuses</param>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get(Default 20, Max 40)</param>
        public Task<ResponseCollection<Status>> GetStatuses(int id
                                                     , bool? onlyMedia = default(bool?), bool? excludeReplies = default(bool?)
                                                     , int? maxId = default(int?), int? sinceId = default(int?)
                                                     , int limit = 20)
        {
            var query = new Dictionary<string, object>
            {
                {"only_media", onlyMedia},
                {"exclude_replies", excludeReplies},
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Status>(FullUrl(string.Format(ApiMethods.GetStatuses, id), query));
        }

        /// <summary>
        /// Following an account
        /// </summary>
        /// <returns>The target account's Relationship</returns>
        /// <param name="id">Target AccountID</param>
        public Task<Relationship> Follow(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Follow, id));
        }

        /// <summary>
        /// Unfollow an account
        /// </summary>
        /// <returns>The target account's Relationship</returns>
        /// <param name="id">Target AcconutID</param>
        public Task<Relationship> Unfollow(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unfollow, id));
        }

        /// <summary>
        /// Blocking an account
        /// </summary>
        /// <returns>The target account's Relationship</returns>
        /// <param name="id">Target AccountID</param>
        public Task<Relationship> Block(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Block, id));
        }

        /// <summary>
        /// Unblocking an account
        /// </summary>
        /// <returns>The target account's Relationship</returns>
        /// <param name="id">Target AccountID</param>
        public Task<Relationship> Unblock(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unblock, id));
        }

        /// <summary>
        /// Muting an account
        /// </summary>
        /// <returns>The target account's Relationship</returns>
        /// <param name="id">Target AccountID</param>
        public Task<Relationship> Mute(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Mute, id));
        }

        /// <summary>
        /// Unmuting an acocunt
        /// </summary>
        /// <returns>The target account's Relationship</returns>
        /// <param name="id">Target AccountID</param>
        public Task<Relationship> Unmute(int id)
        {
            return GetClient().Post<Relationship>(string.Format(ApiMethods.Unmute, id));
        }

        /// <summary>
        /// Getting an account's relationships
        /// </summary>
        /// <returns>The relationships that given ID</returns>
        /// <param name="ids">Target AccountsID array</param>
        public Task<IEnumerable<Relationship>> GetRelationships(IEnumerable<int> ids)
        {
            var query = new Dictionary<string, object>
            {
                {"id", ids}
            }.ToQueryString();

            return GetClient().Get<IEnumerable<Relationship>>(FullUrl(ApiMethods.GetRelationships, query));
        }

        /// <summary>
        /// Searching for accounts
        /// </summary>
        /// <returns>The account.</returns>
        /// <param name="searchQuery">Search query.</param>
        /// <param name="limit">Maximum number of matching accounts to return (default: 40)</param>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        public Task<ResponseCollection<Account>> SearchAccount(string searchQuery
                                                        , int limit = 40
                                                        , int? maxId = default(int?), int? sinceId = default(int?))
        {
            var query = new Dictionary<string, object>
            {
                {"q", searchQuery},
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Account>(FullUrl(ApiMethods.SearchForAccounts, query));
        }

        /// <summary>
        /// Fetching a user's blocks
        /// </summary>
        /// <returns>The blocked users Account</returns>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 40, Max 80)</param>
        public Task<ResponseCollection<Account>> GetBlockedUsers(int? maxId = default(int?), int? sinceId = default(int?)
                                                          , int limit = 40)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Account>(FullUrl(ApiMethods.GetBlocks, query));
        }

        /// <summary>
        /// Fetching a user's favourites
        /// </summary>
        /// <returns>The favourited Statuses</returns>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 20, Max 40)</param>
        public Task<ResponseCollection<Status>> GetFavourites(int? maxId = default(int?), int? sinceId = default(int?)
                                                       , int limit = 20)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Status>(FullUrl(ApiMethods.GetFavourites, query));
        }

        /// <summary>
        /// Fetching a list of follow requests
        /// </summary>
        /// <returns>Accounts who requested to follow</returns>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 40, Max 80)</param>
        public Task<ResponseCollection<Account>> GetFollowRequests(int? maxId = default(int?), int? sinceId = default(int?)
                                                            , int limit = 40)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Account>(FullUrl(ApiMethods.GetFollowRequests, query));
        }

        /// <summary>
        /// Authorizing follow requests
        /// </summary>
        /// <returns>None</returns>
        /// <param name="id">Target AccountID</param>
        public Task AuthorizeFollowRequest(int id)
        {
            var param = new Dictionary<string, object>
            {
                {"id", id}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

            return GetClient().Post(string.Format(ApiMethods.AuthorizeFollowRequest, id));
        }

        /// <summary>
        /// Rejecting follow requests
        /// </summary>
        /// <returns>None</returns>
        /// <param name="id">Target AccountID</param>
        public Task RejectFollowRequest(int id)
        {
            var param = new Dictionary<string, object>
            {
                {"id", id}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

            return GetClient().Post(string.Format(ApiMethods.RejectFollowRequest, id));
        }

        /// <summary>
        /// Following a remote user
        /// </summary>
        /// <returns>The follows Account</returns>
        /// <param name="fullUserId">"username@domain" formatted Target UserName</param>
        public Task<Account> RemoteFollow(string fullUserId)
        {
            var param = new Dictionary<string, object>
            {
                {"uri", fullUserId}
            }.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString()));

            return GetClient().Post<Account>(ApiMethods.RemoteFollow);
        }

        /// <summary>
        /// Getting instance information
        /// </summary>
        /// <returns>Current Instance.</returns>
        public Task<Instance> GetInstance()
        {
            // Does not require authentication
            return GetClient().Get<Instance>(ApiMethods.GetInstance);
        }
#if !NETSTANDARD1_1
        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <returns>The media Attachment</returns>
        /// <param name="filePath">Media's file path</param>
        public Task<Attachment> UploadMedia(string filePath)
        {
            return GetClient().PostWithMedia<Attachment>(ApiMethods.UploadMedia, filePath);
        }
#endif
        /// <summary>
        /// Uploading a media attachment
        /// </summary>
        /// <returns>The media Attachment</returns>
        /// <param name="image">byte array of Image</param>
        public Task<Attachment> UploadMedia(byte[] image)
        {
            return GetClient().PostWithMedia<Attachment>(ApiMethods.UploadMedia, image);
        }

        /// <summary>
        /// Fetching a user's mutes
        /// </summary>
        /// <returns>The muted Accounts</returns>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 40, Max 80)</param>
        public Task<ResponseCollection<Account>> GetMutes(int? maxId = default(int?), int? sinceId = default(int?)
                                                   , int limit = 40)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Account>(FullUrl(ApiMethods.GetMutes, query));
        }

        /// <summary>
        /// Fetching a user's notifications
        /// </summary>
        /// <returns>The Notifications.</returns>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 15, Max 30)</param>
        public Task<ResponseCollection<Notification>> GetNotifications(int? maxId = default(int?), int? sinceId = default(int?)
                                                                , int limit = 15)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Notification>(FullUrl(ApiMethods.GetNotifications, query));
        }

        /// <summary>
        /// Getting a single notification
        /// </summary>
        /// <returns>The single Notification</returns>
        public Task<Notification> GetSingleNotification(int id)
        {
            return GetClient().Get<Notification>(string.Format(ApiMethods.GetSingleNotification, id));
        }

        /// <summary>
        /// Clearing notifications
        /// </summary>
        /// <returns>None</returns>
        public Task ClearNotifications()
        {
            return GetClient().Post(ApiMethods.ClearNotifications);
        }

        /// <summary>
        /// Fetching a user's reports
        /// </summary>
        /// <returns>The Reports that current user made</returns>
        public Task<IEnumerable<Report>> GetReports()
        {
            return GetClient().Get<IEnumerable<Report>>(ApiMethods.GetReports);
        }

        /// <summary>
        /// Reporting a user
        /// </summary>
        /// <returns>Finished Report</returns>
        /// <param name="accountId">Target AccountID</param>
        /// <param name="statusIds">Target StatusIDs array</param>
        /// <param name="comment">Comment associated this report</param>
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

        /// <summary>
        /// Searching for content
        /// </summary>
        /// <returns>The search Results</returns>
        /// <param name="searchQuery">Search query.</param>
        /// <param name="searchGlobal">If set to <c>true</c> search non-local accounts</param>
        public Task<Results> Search(string searchQuery, bool searchGlobal = false)
        {
            var query = new Dictionary<string, object>
            {
                {"q", searchQuery},
                {"resolve", searchGlobal},
            }.ToQueryString();

            return GetClient().Get<Results>(FullUrl(ApiMethods.Search, query));
        }

        /// <summary>
        /// Fetching a status
        /// </summary>
        /// <returns>The Status</returns>
        /// <param name="id">Target StatusID</param>
        public Task<Status> GetStatus(int id)
        {
            return GetClient().Get<Status>(string.Format(ApiMethods.GetStatus, id));
        }

        /// <summary>
        /// Getting status context
        /// </summary>
        /// <returns>The staus Context</returns>
        /// <param name="id">Target StatusID</param>
        public Task<Context> GetStausContext(int id)
        {
            return GetClient().Get<Context>(string.Format(ApiMethods.GetStatusContext, id));
        }

        /// <summary>
        /// Getting a card accociated with a status
        /// </summary>
        /// <returns>The status Card</returns>
        /// <param name="id">Target StatusID</param>
        public Task<Card> GetStatusCard(int id)
        {
            return GetClient().Get<Card>(string.Format(ApiMethods.GetStatusCard, id));
        }

        /// <summary>
        /// Getting who reblogged a status
        /// </summary>
        /// <returns>Accounts who reblogged</returns>
        /// <param name="id">Target StatusID</param>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">Since ID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 40, Max 80)</param>
        public Task<ResponseCollection<Account>> GetStatusRebloggedAccounts(int id
                                                                    , int? maxId = default(int?), int? sinceId = default(int?)
                                                                    , int limit = 40)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().GetCollection<Account>(FullUrl(string.Format(ApiMethods.GetStatusRebloggedBy, id), query));
        }

        /// <summary>
        /// Getting who favourited a status
        /// </summary>
        /// <returns>Accounts who favourited</returns>
        /// <param name="id">Target AccountID</param>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        public Task<ResponseCollection<Account>> GetStatusFavouritedAccounts(int id
                                                                     , int? maxId = default(int?), int? sinceId = default(int?)
                                                                     , int limit = 40)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }
                .AddRangeParameter(maxId, sinceId)
                .ToQueryString();

            return GetClient().GetCollection<Account>(FullUrl(string.Format(ApiMethods.GetStatusFavouritedBy, id), query));
        }

        /// <summary>
        /// Posting a new status
        /// </summary>
        /// <returns>The new Status</returns>
        /// <param name="status">Status text</param>
        /// <param name="inReplyToId">StatusID tha you want to reply to</param>
        /// <param name="mediaIds">mediaID to attach to the Status(maximum 4)</param>
        /// <param name="sensitive">if this status media as NSFW, set <c>true</c></param>
        /// <param name="spoilerText">warning text</param>
        /// <param name="visibility">Visibility.</param>
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

        /// <summary>
        /// Deketing a status
        /// </summary>
        /// <returns>None</returns>
        /// <param name="id">Target StatusID</param>
        public Task DeleteStatus(int id)
        {
            return GetClient().Delete(string.Format(ApiMethods.DeleteStatus, id));
        }

        /// <summary>
        /// Rebloggin a status
        /// </summary>
        /// <returns>The reblogged Status</returns>
        /// <param name="id">Target StatusID</param>
        public Task<Status> Reblog(int id)
        {
            return GetClient().Post<Status>(string.Format(ApiMethods.ReblogStatus, id));
        }

        /// <summary>
        /// Unreblogging a status
        /// </summary>
        /// <returns>The unreblogged Status</returns>
        /// <param name="id">Target StatusID</param>
        public Task<Status> Unreblog(int id)
        {
            return GetClient().Post<Status>(string.Format(ApiMethods.UnreblogStatus, id));
        }

        /// <summary>
        /// Favouriting a status
        /// </summary>
        /// <returns>The favourited Status</returns>
        /// <param name="id">Target StatusID</param>
        public Task<Status> Favourite(int id)
        {
            return GetClient().Post<Status>(string.Format(ApiMethods.FavouritingStatus, id));
        }

        /// <summary>
        /// Unfavouriting a status
        /// </summary>
        /// <returns>The unfavourited Status</returns>
        /// <param name="id">Target StatusID</param>
        public Task<Status> Unfavourite(int id)
        {
            return GetClient().Post<Status>(string.Format(ApiMethods.UnfavouritingStatus, id));
        }

        /// <summary>
        /// Retrieving a timeline
        /// </summary>
        /// <returns>The recent home timeline Status</returns>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 20, Max 40)</param>
        public Task<ResponseCollection<Status>> GetRecentHomeTimeline(int? maxId = default(int?), int? sinceId = default(int?)
                                                                      , int limit = 20)
        {
            var query = new Dictionary<string, object>
            {
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Status>(FullUrl(ApiMethods.GetHomeTimeline, query));
        }

        /// <summary>
        /// Retrieving a timeline
        /// </summary>
        /// <returns>The recent public timeline Status</returns>
        /// <param name="local">If set to <c>true</c> show this Host only</param>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        /// <param name="limit">Maximum number of accounts to get (Default 20, Max 40)</param>
        public Task<ResponseCollection<Status>> GetRecentPublicTimeline(bool? local = null
                                                                        , int? maxId = default(int?), int? sinceId = default(int?)
                                                                        , int limit = 20)
        {
            var query = new Dictionary<string, object>
            {
                {"local", local},
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Status>(FullUrl(ApiMethods.GetPublicTimeline, query));
        }

        /// <summary>
        /// Retrieving a timeline
        /// </summary>
        /// <returns>The recent hashtag timeline.</returns>
        /// <param name="hashtag">Hashtag.</param>
        /// <param name="local">If set to <c>true</c> show this Host only</param>
        /// <param name="maxId">MaxID</param>
        /// <param name="sinceId">SinceID</param>
        public Task<ResponseCollection<Status>> GetRecentHashtagTimeline(string hashtag
                                                                         , bool? local = null
                                                                         , int? maxId = default(int?), int? sinceId = default(int?)
                                                                         , int limit = 20)
        {
            var query = new Dictionary<string, object>
            {
                {"local", local},
                {"limit", limit}
            }.AddRangeParameter(maxId, sinceId)
             .ToQueryString();

            return GetClient().GetCollection<Status>(FullUrl(string.Format(ApiMethods.GetHastagTimeline, hashtag), query));
        }

        /// <summary>
        /// Gets the observable home timeline as Observable
        /// </summary>
        /// <returns>The observable home timeline.</returns>
        /// <param name="host">Stream host URL if stream provide subdomain</param>
        public IObservable<IStreamEntity> GetObservableHomeTimeline(string host = null)
        {
            return GetClient(host).GetObservable(ApiMethods.GetUserStream);
        }

        /// <summary>
        /// Gets the observable public timeline as Observable
        /// </summary>
        /// <returns>The observable public timeline.</returns>
        /// <param name="local">If set to <c>true</c> show this Host only</param>
        /// <param name="host">Stream host URL if stream provide subdomain</param>
        public IObservable<IStreamEntity> GetObservablePublicTimeline(bool local = false, string host = null)
        {
            var query = new Dictionary<string, object>
            {
                {"local", local},
            }.ToQueryString();

            return GetClient(host).GetObservable(FullUrl(ApiMethods.GetPublicStream, query));
        }

        /// <summary>
        /// Gets the observable hashtag timeline as Observable
        /// </summary>
        /// <returns>The observable hashtag timeline.</returns>
        /// <param name="hashtag">Hashtag.</param>
        /// <param name="local">If set to <c>true</c>  show this Host only</param>
        /// <param name="host">Stream host URL if stream provide subdomain</param>
        public IObservable<IStreamEntity> GetObservableHashtagTimeline(string hashtag, bool local = false, string host = null)
        {
            var query = new Dictionary<string, object>
            {
                {"tag", hashtag},
                {"local", local}
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
