# Mastodot - Mastodon API library for C# (.NET)

 [![NuGetBadge](https://img.shields.io/nuget/v/Mastodot.svg)](https://www.nuget.org/packages/Mastodot)


Mastodot は C# .NET Standard 1.1 および 1.3 で開発されている Mastodon API にアクセスするためのライブラリです．
その他に 画像投稿 や 各種トークンの保存，読み込み(予定) の機能も提供しています．

## 使用例

トークンの取得，トゥート，ストリームへの接続 の例を一つのアプリケーションに実装している完全なコードは example ディレクトリを参照してください．

## 使い方

### Mastodon Instance へのアプリケーションの登録

Mastodon の API を使用するには ClientID と ClientSecret が必要になるため，その取得を行います． 

本ライブラリでは `ApplicationManager` クラスの `RegistApp` メソッドを通して行います．

```csharp

var registeredApp = await ApplicationManager.RegistApp("Host name", "Your Application Name", Scope.Read | Scope.Write | Scope.Follow);

```

ここで得られた `Entities.RegisteredApp` クラスのインスタンスである `registeredApp ` に ClientID や ClientKey などの情報が含まれているため，`MastodonJsonConverter` クラスの `TrySerialize` メソッドなどでシリアライズしたものをファイルに保存するなどを行ってください．

### アクセストークンの取得

アクセストークンの取得には 2種類 方法があり，

* このライブラリを通して登録に使用したメールアドレスとパスワードを使用して取得
* ブラウザでログインした後特定のURLに飛ぶことで認可し取得

が可能です．

#### メールアドレスを使用

```csharp

var tokens = ApplicaionManager.GetAccessTokenByEmail(registeredApp, "Email", "Password");
// RegisteredApp インスタンスがなくてもトークンを覚えているのであれば以下も可
var tokens = ApplicationManager.GetAccessTokenByEmail("Host name", "ClientID", "ClientSecret", Scope, "Email", "Password");

```

#### OAuth を使用

```csharp

var url = ApplicationManager.GetOAuthUrl(registeredApp);
// ブラウザなどでこの URL にアクセスし，表示された文字列をコピーするなどして取得

var tokens = await ApplicationManager.GetAccessTokenByCode(registeredApp, "Code that Browser shows");

```

### アクセストークンを使用した Mastodon API へのアクセス

以上で得られた `Entities.TokenInfo` クラスのインスタンスである `tokens` のメンバの `AccessToken` を使用して Mastodon の API を使用します．

そのためのクライアントは

```csharp

var client = new MastodonClient("Host name", "AccessToken");

```

で生成することが出来ます．

#### Toot してみる

Toot するには `PostNewStatus` メソッドを通して行います．

```csharp

client.PostNewStatus(status: "Hello Mastodon!");
// 画像つきで行うのであれば
var attachment = await client.UploadMedia("File Path");
client.PostNewStatus("Look my cuuuuute dog!", mediaIds: new int[]{attachment.Id});

```

のようにして行うことが可能です．

#### ストリーム API を使う

Mastodon では Twitter の用にタイムラインなどのストリームを扱うことが出来ます．

このストリームを扱う場合は以下のように行います．

```csharp

var publicStream = client.GetObservablePublicTimeline()
                    .OfType<Status>()
                    .Subscribe(x => Console.WriteLine($"{x.Account.FullUserName} Tooted: {x.Content}"));

```

`GetObservable{User, Public, Hashtag}Timeline` は `IObservable<IStreamEntity>` を返すので Subscribe して Stream の情報を取得します．

このためストリームを扱う際は `System.Reactive` を使用することをおすすめします(Nuget などで導入してください)．
`IObserver<IStreamEntity>` を実装したクラスを使用して取得することも可能だと思います．

#### 細かくタイムラインをフェッチする

Link ヘッダに対応しているため，`max_id` や `since_id` に対応している API を使う際は `Links` パラメータの `Next` や `Prev` の ID を使用して連続的に Status などを取得することが出来ます．

Twitter の Pull-to-refresh のように最新のツイートを読み込むような機能などに使われます．

```csharp

var statuses = await client.GetRecentPublicTimeline(sinceId: 1192);
// do show some statuses
statuses = await client.GetRecentPublicTimeline(sinceId: statuses.Links.Prev.Value);
// do show next statuses

```

## ライセンス

MIT

## その他

PR は大歓迎です
