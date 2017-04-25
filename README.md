# Mastodot - Mastodon API library for C# (.NET)

 [![NuGetBadge](https://img.shields.io/nuget/v/Mastodot.svg)](https://www.nuget.org/packages/Mastodot)

日本語は[こちら](https://github.com/yamachu/Mastodot/blob/master/README.ja.md)


## Sample code

See [example](https://github.com/yamachu/Mastodot/blob/master/example/Program.cs)

Sample code contains how to generate tokens, toot, and subscribe stream.

### Regist an app to Mastodon Instance

```csharp

var registeredApp = await ApplicationManager.RegistApp("Host name" /* ex: mastodon.cloud */, "Your Application Name", Scope.Read | Scope.Write | Scope.Follow);

```

registeredApp contains ClienID and ClientSecret.

### Login and get AccessToken

```csharp

// if login by email
var tokens = ApplicaionManager.GetAccessTokenByEmail(registeredApp, "Email", "Password");

// if login by use OAuth
var url = ApplicationManager.GetOAuthUrl(registeredApp);
// access this url via browser, HttpClient, ...
var tokens = await ApplicationManager.GetAccessTokenByCode(registeredApp, "Code that Browser shows");

```

tokens contains AccessToken.
You can access Mastodon API by using this AccessToken.

#### Using Mastodon API

This code is how to Toot.

```csharp

var client = new MastodonClient("Host name (url)", tokens.AccessToken);
client.PostNewStatus(status: "Hello Mastodon!");
// With media
var attachment = await client.UploadMedia("File Path");
client.PostNewStatus("Look my cuuuuute dog!", mediaIds: new int[]{attachment.Id});

```

and Using StreamAPI

```csharp

var publicStream = client.GetObservablePublicTimeline();
                    .OfType<Status>()
                    .Subscribe(x => Console.WriteLine($"{x.Account.FullUserName} Tooted: {x.Content}"));

```

## License

MIT

## Other

Welcode pull requests!
