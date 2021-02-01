using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.StringToHttpRequestExtensionsTests
{
    public class StringToHttpRequestExtensionTests : UnitTest
    {
        private const string Url = "http://google.com";

        public static IEnumerable<object[]> StringIntoRequestParams = new[]
        {
            new object[] { (Func<string, HttpClient, HttpRequest>)((x, h) => x.AsAGetRequest(h)), HttpMethod.Get },
            new object[] { (Func<string, HttpClient, HttpRequest>)((x, h) => x.AsADeleteRequest(h)), HttpMethod.Delete },
            new object[] { (Func<string, HttpClient, HttpRequest>)((x, h) => x.AsAHeadRequest(h)), HttpMethod.Head },
            new object[] { (Func<string, HttpClient, HttpRequest>)((x, h) => x.AsAPatchRequest(h)), HttpMethod.Patch },
            new object[] { (Func<string, HttpClient, HttpRequest>)((x, h) => x.AsAnOptionsRequest(h)), HttpMethod.Options },
            new object[] { (Func<string, HttpClient, HttpRequest>)((x, h) => x.AsAPostRequest(h)), HttpMethod.Post },
            new object[] { (Func<string, HttpClient, HttpRequest>)((x, h) => x.AsAPutRequest(h)), HttpMethod.Put },
            new object[] { (Func<string, HttpClient, HttpRequest>)((x, h) => x.AsATraceRequest(h)), HttpMethod.Trace }
        };

        [Theory]
        [MemberData(nameof(StringIntoRequestParams))]
        public async Task Can_Convert_A_String_Into_A_Request(Func<string, HttpClient, HttpRequest> func, HttpMethod expected)
        {
            OnRequestMade(r => Assert.Equal(expected, r.Method));

            await func(Url, HttpClient).EnsureSuccessStatusCodeAsync();
        }
    }
}
