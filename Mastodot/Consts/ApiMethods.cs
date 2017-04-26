using System;
namespace Mastodot.Consts
{
    internal class ApiMethods
    {
        #region Accounts
        public static readonly string GetAccount = "/api/v1/accounts/{0}";
        public static readonly string GetCurrentAccount = "/api/v1/accounts/verify_credentials";
        public static readonly string UpdateCurrentAccount = "/api/v1/accounts/update_credentials";
        public static readonly string GetFollowers = "/api/v1/accounts/{0}/followers";
        public static readonly string GetFollowing = "/api/v1/accounts/{0}/following";
        public static readonly string GetStatuses = "/api/v1/accounts/{0}/statuses";
        public static readonly string Follow = "/api/v1/accounts/{0}/follow";
        public static readonly string Unfollow = "/api/v1/accounts/{0}/unfollow";
        public static readonly string Block = "/api/v1/accounts/{0}/block";
        public static readonly string Unblock = "/api/v1/accounts/{0}/unblock";
        public static readonly string Mute = "/api/v1/accounts/{0}/mute";
        public static readonly string Unmute = "/api/v1/accounts/{0}/unmute";
        public static readonly string GetRelationships = "/api/v1/accounts/relationships";
        public static readonly string SearchForAccounts = "/api/v1/accounts/search";
        #endregion

        #region Apps
        public static readonly string RegistApp = "/api/v1/apps";
        #endregion

        #region Blocks
        public static readonly string GetBlocks = "/api/v1/blocks";
        #endregion

        #region Favourites
        public static readonly string GetFavourites = "/api/v1/favourites";
        #endregion

        #region FollowRequests
        public static readonly string GetFollowRequests = "/api/v1/follow_requests";
        public static readonly string AuthorizeFollowRequest = "/api/v1/follow_requests/authorize";
        public static readonly string RejectFollowRequest = "/api/v1/follow_requests/reject";
        #endregion

        #region Follows
        public static readonly string RemoteFollow = "/api/v1/follows";
        #endregion

        #region Instances
        public static readonly string GetInstance = "/api/v1/instance";
        #endregion

        #region Media
        public static readonly string UploadMedia = "/api/v1/media";
        #endregion

        #region Mutes
        public static readonly string GetMutes = "/api/v1/mutes";
        #endregion

        #region Notifications
        public static readonly string GetNotifications = "/api/v1/notifications";
        public static readonly string GetSingleNotification = "/api/v1/notifications/{0}";
        public static readonly string ClearNotifications = "/api/v1/notifications/clear";
        #endregion

        #region Reports
        public static readonly string GetReports = "/api/v1/reports";
        public static readonly string ReportUser = "/api/v1/reports";
        #endregion

        #region Search
        public static readonly string Search = "/api/v1/search";
        #endregion

        #region Statuses
        public static readonly string GetStatus = "/api/v1/statuses/{0}";
        public static readonly string GetStatusContext = "/api/v1/statuses/{0}/context";
        public static readonly string GetStatusCard = "/api/v1/statuses/{0}/card";
        public static readonly string GetStatusRebloggedBy = "/api/v1/statuses/{0}/reblogged_by";
        public static readonly string GetStatusFavouritedBy = "/api/v1/statuses/{0}/favourited_by";
        public static readonly string PostNewStatus = "/api/v1/statuses";
        public static readonly string DeleteStatus = "/api/v1/statuses/{0}";
        public static readonly string ReblogStatus = "/api/v1/statuses/{0}/reblog";
        public static readonly string UnreblogStatus = "/api/v1/statuses/{0}/unreblog";
        public static readonly string FavouritingStatus = "/api/v1/statuses/{0}/favourite";
        public static readonly string UnfavouritingStatus = "/api/v1/statuses/{0}/unfavourite";
        #endregion

        #region Timelines
        public static readonly string GetHomeTimeline = "/api/v1/timelines/home";
        public static readonly string GetPublicTimeline = "/api/v1/timelines/public";
        public static readonly string GetHastagTimeline = "/api/v1/timelines/tag/{0}";
        #endregion

        #region Streaming
        public static readonly string GetUserStream = "/api/v1/streaming/user";
        public static readonly string GetPublicStream = "/api/v1/streaming/public";
        public static readonly string GetHashTagStream = "/api/v1/streaming/hashtag";
        #endregion

        #region OAuth
        public static readonly string OAuthAuthorize = "/oauth/authorize";
        public static readonly string OAuthToken = "/oauth/token";
        #endregion

        #region Custom
        public static readonly string APIBaseUrl = "/api/v1/{0}";
        public static readonly string StreamBaseUrl = "/api/v1/streaming/{0}";
        #endregion
    }
}
