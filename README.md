# FluentHttpExtensions

> Syntactic sugar in a single file for the `System.Net.Http.HttpClient` class.

## Introduction

FluentHttpExtensions is a single file fluent interface for the `System.Net.Http.HttpClient` class. This means that you can simply
drop the `HttpRequest.cs` file into your project and begin making http requests easily and beautifully.

I built this for one reason: to stop repeating myself whenever I'm working on a library or project where one of the
requirements is to keep external dependencies to a minimum, or where a fully featured request library such as
`Flurl.Http` feels a little overkill.

This library in its single file and packaged form requires your project to be using .NET Standard 2.0 or above. Where you are dropping the file into your project, you will also need to install the System.Text.Json package.

### Getting Started

The entry point to FluentHttpExtensions is the `HttpRequest` class. This class should be initialized and used for one request
only. It has two constructors:

```
public HttpRequest(string uri)

// and...

public HttpRequest(string uri, HttpClient? client = default)
```

You can also construct a `HttpRequest` instance from a string representing a UI, for example:

```
"https://www.google.com".AsAGetRequest() // yields a HttpRequest
```

To see more on the different methods available to modify the HTTP verb, read on.

## Methods

**Request Methods**

* `AsAGetRequest()` - Sets the verb of the request to GET.

* `AsAPostRequest()` - Sets the verb of the request to POST.

* `AsAPutRequest()` - Sets the verb of the request to PUT.

* `AsAPatchRequest()` - Sets the verb of the request to PATCH.

* `AsADeleteRequest()` - Sets the verb of the request to DELETE.

* `AsATraceRequest()` - Sets the verb of the request to TRACE.

* `AsAHeadRequest()` - Sets the verb of the request to HEAD.

* `AsAnOptionsRequest()` - Sets the verb of the request to OPTIONS.

**String to Request Methods**

* `"abc".AsAGetRequest(HttpClient? client = default)` - Creates a GET request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"abc".AsAPostRequest(HttpClient? client = default)`  - Creates a POST request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"abc".AsAPutRequest(HttpClient? client = default)` - Creates a PUT request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"abc".AsAPatchRequest(HttpClient? client = default)` - Creates a PATCH request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"abc".AsADeleteRequest(HttpClient? client = default)` - Creates a DELETE request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"abc".AsATraceRequest(HttpClient? client = default)` - Creates a TRACE request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"abc".AsAHeadRequest(HttpClient? client = default)` - Creates a HEAD request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

* `"abc".AsAnOptionsRequest(HttpClient? client = default)` - Creates an OPTIONS request with an optional `HttpClient` and returns a configured `HttpRequest` instance.

**URL Methods**

* `WithPathSegment([short, ushort, int, uint, long, ulong, Guid, string?] pathSegment)` - Adds a path segment of the
specified type to the URL. They are added in the order that `WithPathSegment` is called.

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

**Send Triggering Methods**

The send triggering methods perform the actual request to the configured endpoint. For that reason, they do not
continue the fluent interface and return response types relevant to their methods.

* `Task<HttpResponseMessage> MakeRequest()` - Performs the request using the configured parameters and returns the
response.

* `Task EnsureSuccessStatusCode()` - Performs the request and calls `.EnsureSuccessStatusCode()` on the returned
response.

* `Task<string> ReadResponseAsString()` - Performs the request and reads the request as a string. This method first
ensures that the status code returned is a successful one.

* `Task<T> ReadJsonResponseAs<T>()` - Performs the request and deserializes the JSON response into an object of type T.
This method first ensures that the status code returned is a successful one.

* `Task<T> ReadXmlResponseAs<T>()` - Performs the request and Deserializes the XML content of the response into an
object of type T. This method first ensures that the status code returned is a successful one.

**HttpResponseMessage Methods**

Occasionally, you may want to perform more than one action on a `HttpResponseMessage` returned by the `MakeRequest`
method. The following methods allow you to do that against a `HttpResponseMessage` saved in a variable multiple times,
without having to make the request again.

* `Task<string> ReadResponseAsString()` - Reads the content of the response as a string.

* `Task<T> ReadJsonResponseAs<T>()` - Deserializes the JSON content of the response into an object of type T.

* `Task<T> ReadXmlResponseAs<T>()` - Deserializes the XML content of the response into an object of type T.

## Examples

**Daily cat facts**

```csharp
var catFacts = await "https://cat-fact.herokuapp.com"
    .AsAGetRequest()
    .WithPathSegment("facts")
    .ReadResponseAsString();

Console.WriteLine(catFacts);

> {"all":[{"_id":"58e008ad0aac31001185ed0c","text":"The frequency of a domestic cat's purr is the same at which ..........

```
