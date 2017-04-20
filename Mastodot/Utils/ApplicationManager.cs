using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mastodot.Consts;
using Mastodot.Entities;
using Mastodot.Enums;

namespace Mastodot.Utils
{
    public class ApplicaionManager
    {
        private static readonly string NoRedirect = "urn:ietf:wg:oauth:2.0:oob";

        public static async Task<RegisteredApp> RegistApp(string host, string clientName, Scope scope, string redirectUri = null, string website = null, string subdomain = null)
        {
            var param = new Dictionary<string, string>
            {
                {"client_name", clientName},
                {"scopes", scope.ToString(encoding: false)},
                {"redirect_uris", string.IsNullOrEmpty(redirectUri)
                                       ? NoRedirect
                                           : redirectUri}
            };
            if (!string.IsNullOrEmpty(website))
            {
                param.Add("website", website);
            }
            var content = new FormUrlEncodedContent(param);

            var appsSubdomain = string.IsNullOrEmpty(subdomain) ? "" : subdomain + ".";
            var url = $"https://{appsSubdomain}{host}{ApiMethods.RegistApp}";

            var client = new HttpClient();
            var response = await client.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var registerdApp = MastodonJsonConverter.TryDeserialize<RegisteredApp>(responseBody);
            registerdApp.Host = host;
            registerdApp.Scope = scope;

            return registerdApp;
        }

        public static string GetOAuthUrl(RegisteredApp app, string subdomain = null)
        {
            return GetOAuthUrl(app.Host, app.ClientId, app.Scope, app.RedirectUri, subdomain);
        }

        public static string GetOAuthUrl(string host, string clientId, Scope scope, string redirectUri = null, string subdomain = null)
        {
            var oauthSubdomain = string.IsNullOrEmpty(subdomain) ? "" : subdomain + ".";
            var query = $"?response_type=code&client_id={clientId}&scope={scope.ToString(encoding: true)}&redirect_uri={(string.IsNullOrEmpty(redirectUri) ? NoRedirect : redirectUri)}";
            return $"https://{oauthSubdomain}{host}{ApiMethods.OAuthAuthorize}{query}";
        }

        public static async Task<TokenInfo> GetAccessTokenByEmail(RegisteredApp app, string email, string password, string subdomain = null)
        {
            return await GetAccessTokenByEmail(app.Host, app.ClientId, app.ClientSecret, app.Scope, email, password, subdomain);
        }

        public static async Task<TokenInfo> GetAccessTokenByEmail(string host, string clientId, string clientSecret, Scope scope, string email, string password, string subdomain = null)
        {
            var param = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"grant_type", "password"},
                {"username", email},
                {"password", password},
                {"scope", scope.ToString(encoding: false)}
            };
            var content = new FormUrlEncodedContent(param);

            var oauthSubdomain = string.IsNullOrEmpty(subdomain) ? "" : subdomain + ".";
            var url = $"https://{oauthSubdomain}{host}{ApiMethods.OAuthToken}";

            var client = new HttpClient();
            var response = await client.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            return MastodonJsonConverter.TryDeserialize<TokenInfo>(responseBody);
        }

        public static async Task<TokenInfo> GetAccessTokenByCode(RegisteredApp app, string code, string redirectUri = null, string subdomain = null)
        {
            return await GetAccessTokenByCode(app.Host, app.ClientId, app.ClientSecret, code, redirectUri, subdomain);
        }

        public static async Task<TokenInfo> GetAccessTokenByCode(string host, string clientId, string clientSecret, string code, string redirectUri = null, string subdomain = null)
        {
            var param = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"grant_type", "authorization_code"},
                {"code", code.Trim()},
                {"redirect_uri", string.IsNullOrEmpty(redirectUri)
                                       ? NoRedirect
                                           : redirectUri}
            };
            var content = new FormUrlEncodedContent(param);

            var oauthSubdomain = string.IsNullOrEmpty(subdomain) ? "" : subdomain + ".";
            var url = $"https://{oauthSubdomain}{host}{ApiMethods.OAuthToken}";

            var client = new HttpClient();
            var response = await client.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            return MastodonJsonConverter.TryDeserialize<TokenInfo>(responseBody);
        }
    }
}
