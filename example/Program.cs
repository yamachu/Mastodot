using System;
using Mastodot;
using Mastodot.Enums;
using Mastodot.Utils;
using Mastodot.Entities;
using System.Reactive.Linq;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            var main = new Program();
            /* Create and get AccessToken to connect Mastodon Instance */
            main.CreateAppAndAuth();

            /* Toot! */
            //main.LetsToot("Hello World! From Mastodot");

            /* Subscribe timeline */
            //main.SubscribePublicstream();
        }

        private void LetsToot(string content)
        {
            var client = new MastodonClient("Mastodon Instance Url that your app registered"
                                            , "AccessToken");

			client.PostNewStatus(status: content).Wait();
        }

        private void CreateAppAndAuth(string host = "mastodon.cloud", string appName = "MastodotSampleClient")
        {
            var app = ApplicaionManager.RegistApp(host, appName, Scope.Read | Scope.Write | Scope.Follow).Result;
            var url = ApplicaionManager.GetOAuthUrl(app);
            Console.WriteLine(url);
            var code = Console.ReadLine();
            var tokens = ApplicaionManager.GetAccessTokenByCode(app, code).Result;
            Console.WriteLine(tokens.AccessToken);
            Console.ReadKey();
            Console.WriteLine("Finish");
            Console.ReadKey();
        }

        private void SubscribePublicstream()
        {
            var client = new MastodonClient("Mastodon Instance Url that your app registered"
                                            , "AccessToken");

            var statusDs = client.GetObservablePublicTimeline()
                                   .OfType<Status>()
                                   .Subscribe(x => Console.WriteLine($"{x.Account.FullUserName} Tooted: {x.Content}"));

            Console.WriteLine("Press Key then Finish");
            Console.ReadKey();
            statusDs.Dispose();
            Console.WriteLine("Finish");
        }
    }
}
