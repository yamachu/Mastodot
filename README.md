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

### Using Mastodon API

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

var publicStream = client.GetObservablePublicTimeline()
                    .OfType<Status>()
                    .Subscribe(x => Console.WriteLine($"{x.Account.FullUserName} Tooted: {x.Content}"));

```

## MastodonAPIs

### Mastodon APIs

All APIs is ready

Accounts
- [x]  GET /api/v1/accounts/:id
- [x]  GET /api/v1/accounts/verify_credentials
- [x]  PATCH /api/v1/accounts/update_credentials
- [x]  GET /api/v1/accounts/:id/followers
- [x]  GET /api/v1/accounts/:id/following
- [x]  GET /api/v1/accounts/:id/statuses
- [x]  POST /api/v1/accounts/:id/follow
- [x]  POST /api/v1/accounts/:id/unfollow
- [x]  POST /api/v1/accounts/:id/block
- [x]  POST /api/v1/accounts/:id/unblock
- [x]  POST /api/v1/accounts/:id/mute
- [x]  POST /api/v1/accounts/:id/unmute
- [x]  GET /api/v1/accounts/relationships
- [x]  GET /api/v1/accounts/search

Apps
- [x]  POST /api/v1/apps

Blocks
- [x]  GET /api/v1/blocks

Favourites
- [x]  GET /api/v1/favourites

Follow Requests
- [x]  GET /api/v1/follow_requests
- [x]  POST /api/v1/follow_requests/:id/authorize
- [x]  POST /api/v1/follow_requests/:id/reject

Follows
- [x]  POST /api/v1/follows

Instances
- [x]  GET /api/v1/instance

Media
- [x]  POST /api/v1/media

Mutes
- [x]  GET /api/v1/mutes

Notifications
- [x]  GET /api/v1/notifications
- [x]  GET /api/v1/notifications/:id
- [x]  POST /api/v1/notifications/clear

Reports
- [x]  GET /api/v1/reports
- [x]  POST /api/v1/reports

Search
- [x]  GET /api/v1/search

Statuses
- [x]  GET /api/v1/statuses/:id
- [x]  GET /api/v1/statuses/:id/context
- [x]  GET /api/v1/statuses/:id/card
- [x]  GET /api/v1/statuses/:id/reblogged_by
- [x]  GET /api/v1/statuses/:id/favourited_by
- [x]  POST /api/v1/statuses
- [x]  DELETE /api/v1/statuses/:id
- [x]  POST /api/v1/statuses/:id/reblog
- [x]  POST /api/v1/statuses/:id/unreblog
- [x]  POST /api/v1/statuses/:id/favourite
- [x]  POST /api/v1/statuses/:id/unfavourite

Timelines
- [x]  GET /api/v1/timelines/home
- [x]  GET /api/v1/timelines/public
- [x]  GET /api/v1/timelines/tag/:hashtag

Streaming
- [x] GET /api/v1/streaming/user
- [x] GET /api/v1/streaming/public
- [x] GET /api/v1/streaming/hashtag

## License

MIT

## Other

Welcode pull requests!

## For developer

For support multi target frameworks, `Mastodot.csproj` is not supported format.

When edit and debug this library, replace `<TargetFrameworks>netstandard1.1;netstandard1.3</TargetFrameworks>` to `<TargetFramework>netstandard1.1</TargetFramework>` or 
`<TargetFramework>netstandard1.1</TargetFramework>`

