# 👋 Baseline.FluentHttpExtensions

Syntactic sugar in a single file (or a NuGet package) for the `System.Net.Http.HttpClient` class. Part of the Baseline
collection of projects: a set of packages, projects and services to make building modern .NET applications easier.

## 💵 Sponsor

Proudly sponsored by BossLabs Ltd (https://bosslabs.co.uk) - they build scalable software solutions for businesses of
all sizes.

## 🏗 Contributing

Whilst you are free to contribute in the form of issues representing bugs, requests or improvements, I don't accept
code contributions to any of my projects. For more information, click [here](https://github.com/lsymonds#-pull-requests-and-contributions).

## 📖 Introduction

Baseline.FluentHttpExtensions is a single file fluent interface for the `System.Net.Http.HttpClient` class. This means
that you can simply drop the `BaselineFluentHttpExtensions.cs` file into your project (or install the NuGet package) and
begin making http requests easily and beautifully.

I built this for one reason: to stop repeating myself whenever I'm working on a library or project where one of the
requirements is to keep external dependencies to a minimum, or where a fully featured request library such as
`Flurl.Http` feels a little overkill.

This library in its single file and packaged form requires your project to be using .NET Standard 2.0 or above.
Where you are dropping the file into your project, you will also need to install the System.Text.Json package.

### Getting Started

The entry point to Baseline.FluentHttpExtensions is the `HttpRequest` class. This class should be initialized and used f
or one request only. It has two constructors:

```
public HttpRequest(string uri)

// and...

public HttpRequest(string uri, HttpClient? client = default)
```

You can also construct a `HttpRequest` instance from a string representing a UI, for example:

```
"https://www.google.com".AsAGetRequest() // yields a HttpRequest
```

Each request (other than ones triggering a physical request to the configured resource) is fluent, meaning you can
chain multiple method calls together to create code that is not only quick to write but also clean and easy to decipher.

## 🛠 Documentation

### Library methods

**Request Methods**

Request methods modify the HTTP verb of the request.

* `AsAGetRequest()` - Sets the verb of the request to GET.

* `AsAPostRequest()` - Sets the verb of the request to POST.

* `AsAPutRequest()` - Sets the verb of the request to PUT.

* `AsAPatchRequest()` - Sets the verb of the request to PATCH.

* `AsADeleteRequest()` - Sets the verb of the request to DELETE.

* `AsATraceRequest()` - Sets the verb of the request to TRACE.

* `AsAHeadRequest()` - Sets the verb of the request to HEAD.

* `AsAnOptionsRequest()` - Sets the verb of the request to OPTIONS.

**String to Request Methods**

Extension methods of the built in type `string`, these methods allow you to quickly generate a `HttpRequest` instance
without calling the `HttpRequest` constructor. You can optionally pass in a `HttpClient` instance which will be used
instead. If you don't, a static one handled inside of `Baseline.FluentHttpExtensions` will be used instead.

* `"https://www.google.com".AsAGetRequest(HttpClient? client = default)` - Creates a GET request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"https://www.google.com".AsAPostRequest(HttpClient? client = default)`  - Creates a POST request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"https://www.google.com".AsAPutRequest(HttpClient? client = default)` - Creates a PUT request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"https://www.google.com".AsAPatchRequest(HttpClient? client = default)` - Creates a PATCH request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"https://www.google.com".AsADeleteRequest(HttpClient? client = default)` - Creates a DELETE request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"https://www.google.com".AsATraceRequest(HttpClient? client = default)` - Creates a TRACE request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"https://www.google.com".AsAHeadRequest(HttpClient? client = default)` - Creates a HEAD request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"https://www.google.com".AsAnOptionsRequest(HttpClient? client = default)` - Creates an OPTIONS request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

**URL Methods**

URL methods allow you to modify the URL that is sent to

* `WithPathSegment([short, ushort, int, uint, long, ulong, Guid, string?] pathSegment)` - Adds a path segment of the
specified type to the URL. They are added in the order that `WithPathSegment` is called. Most built in types can
be passed in without having to directly convert it to a string.

* `WithQueryParameter(string parameterName, [short, ushort, int, uint, long, ulong, Guid, string?] value)` - Adds a
query string parameter with the specified name and value. This method can be called with the same `parameterName`
values multiple times.

**Header Methods**

* `WithRequestHeader(string headerName, string headerValue)` - adds a header to the request.

* `WithBearerTokenAuth(string token)` - adds a bearer token to the request. It does not matter if you prefix the token
with 'Bearer ' or not.

* `WithBasicAuth(string username, string password)` - converts the username and password specified to a base64 string
which is then prefixed with 'Basic ' and added as the `Authorization` header.

* `WithUserAgent(string userAgent)` - adds a user agent to the request.

* `AcceptingResponseContentType(string contentType, bool replace = false)` - Sets the content type that the requester
(that's you) can interpret. If replace is specified, the `Content-Type` response header is replaced with `contentType`.

* `AcceptingJsonResponseContentType(bool replace = false)` - Sets the content that that the requester can interpet
to `application/json`. If replace is specified, the `Content-Type` response header is replaced with `application/json`.

* `AcceptingXmlResponseContentType(bool replace = false)` - Sets the content that the requester can interpet to
`application/xml`. If replace is specified, the `Content-Type` response header is replaced with `application/xml`.

* `AcceptingPlainResponseContentType(bool replace = false)` - Sets the content that the requester can interpet to
`text/plain`. If replace is specified, the `Content-Type` response header is replaced with `text/plain`.

* `AcceptingHtmlResponseContentType(bool replace = false)` - Sets the content that the requester can interpet to
`text/html`. If replace is specified, the `Content-Type` response header is replaced with `text/html`.

**Body Methods**

* `WithJsonBody<T>(T body)` - Streams the serialized representation of `body` as the request body and sets the
`Content-Type` header to `application/json`.

* `WithTextBody(string body, string contentType = "text/plain")` - Sets the request's body to `body` and sets the
`Content-Type` header to `contentType`.

**Builder Methods**

Builder methods allow you to build components such as the finalised URI this library generates without having to physically
submit the request first. These extension methods are used internally by other extension methods such as `MakeRequest`,
so all responses returned from builder methods are exactly what will be used elsewhere and can be depended on.

* `Uri BuildUri()` - Generates and returns the URI that will be used when the request is sent. Using this method does
not stop you from adding more parameters to the `HttpRequest`, but it will not be updated automatically.

* `string BuildUriAsString()` - Generates and returns the URI that will be used when the request is sent in a string
format. Under the hood, this method utilises the `BuildUri` extension method.

**Send Triggering Methods**

The send triggering methods perform the actual request to the configured endpoint. For that reason, they do not
continue the fluent interface and return response types relevant to their methods.

* `Task<HttpResponseMessage> MakeRequestAsync()` - Performs the request using the configured parameters and returns the
response.

* `Task EnsureSuccessStatusCodeAsync()` - Performs the request and calls `.EnsureSuccessStatusCode()` on the returned
response.

* `Task<string> ReadResponseAsStringAsync()` - Performs the request and reads the request as a string. This method first
ensures that the status code returned is a successful one.

* `Task<T> ReadJsonResponseAsAsync<T>()` - Performs the request and deserializes the JSON response into an object of type T.
This method first ensures that the status code returned is a successful one.

* `Task<T> ReadXmlResponseAsAsync<T>()` - Performs the request and Deserializes the XML content of the response into an
object of type T. This method first ensures that the status code returned is a successful one.

**HttpResponseMessage Methods**

Occasionally, you may want to perform more than one action on a `HttpResponseMessage` returned by the `MakeRequest`
method. The following methods allow you to do that against a `HttpResponseMessage` saved in a variable multiple times,
without having to make the request again.

* `Task<string> ReadResponseAsStringAsync()` - Reads the content of the response as a string.

* `Task<T> ReadJsonResponseAsAsync<T>()` - Deserializes the JSON content of the response into an object of type T.

* `Task<T> ReadXmlResponseAsAsync<T>()` - Deserializes the XML content of the response into an object of type T.

### Controlling the HttpClient instance that is used

Baseline.FluentHttpExtensions allows a pre-defined `HttpClient` instance to be optionally provided in two ways before
defaulting back to its own static, internally managed instance.

You can provide one on each instantiation of a `HttpRequest` class (or via a fluent method that instantiates it for you):

```csharp
var myHttpClient = new HttpClient();

// Via constructor injection.
await ("https://www.google.com", new HttpRequest(myHttpClient))
    .AsAGetRequest()
    .EnsureSuccessStatusCodeAsync();

// Via a fluent method that does the instantiation for you.
await "https://www.google.com"
    .AsAGetRequest(myHttpClient)
    .EnsureSuccessStatusCodeAsync();
```

OR, you can configure one globally via the `BaselineFluentHttpExtensionsHttpClientManager` static class:

```csharp
var myHttpClient = new HttpClient();

BaselineFluentHttpExtensionsHttpClientManager.SetGlobalHttpClient(myHttpClient);

// All future requests where an instance is not specified uses the above defined client.
await "https://www.google.com"
    .AsAGetRequest()
    .EnsureSuccessStatusCodeAsync();

// However, should you want to, you can still override it by providing an instance directly.
var mySecondClient = new HttpClient();

await "https://www.google.com"
    .AsAGetRequest(mySecondClient)
    .EnsureSuccessStatusCodeAsync();
```

## ❔ Examples

```csharp
var user = await "https://jsonplaceholder.typicode.com/users/1"
    .AsAGetRequest()
    .ReadJsonResponseAsAsync<User>();

Console.WriteLine(user.Id);
Console.WriteLine(user.Name);
```
