using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.ActionExtensionsTests
{
    public class RequestMethodTests : UnitTest
    {
        public static IEnumerable<object[]> RequestMethodData = new[]
        {
            new object[] { (Func<HttpRequest, HttpRequest>)(x => x.AsAGetRequest()), HttpMethod.Get },
            new object[] { (Func<HttpRequest, HttpRequest>)(x => x.AsAPostRequest()), HttpMethod.Post },
            new object[] { (Func<HttpRequest, Http.HttpRequest>)(x => x.AsADeleteRequest()), HttpMethod.Delete},
            new object[] { (Func<HttpRequest, Http.HttpRequest>)(x => x.AsAPutRequest()), HttpMethod.Put},
            new object[] { (Func<HttpRequest, Http.HttpRequest>)(x => x.AsAPatchRequest()), new HttpMethod("PATCH") },
            new object[] { (Func<HttpRequest, Http.HttpRequest>)(x => x.AsATraceRequest()), HttpMethod.Trace},
            new object[] { (Func<HttpRequest, Http.HttpRequest>)(x => x.AsAHeadRequest()), HttpMethod.Head},
            new object[] { (Func<HttpRequest, Http.HttpRequest>)(x => x.AsAnOptionsRequest()), HttpMethod.Options},
        };

        [Theory]
        [MemberData(nameof(RequestMethodData))]
        public async Task Can_Set_Request_Methods_Correctly(Func<HttpRequest, HttpRequest> handler, HttpMethod expected)
        {
            // Assert.
            OnRequestMade(r => Assert.Equal(expected.Method, r.Method.Method));

            // Arrange & Act.
            await handler(HttpRequest)
                .EnsureSuccessStatusCode();
        }
    }
}
