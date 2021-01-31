using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit
{
    public class GlobalStaticClientTests : UnitTest
    {
        [Fact]
        public async Task It_Uses_The_Globally_Set_Http_Client_If_Available()
        {
            BaselineFluentHttpExtensionsHttpClientManager.SetGlobalHttpClient(new HttpClient(MessageHandler.Object));

            var messageHandlerResult = ConfigureMessageHandlerResultSuccess(
                MessageHandler,
                "response from a global client",
                "text/plain"
            );
            OnRequestMade(r => Assert.Contains("/global/http/test", r.RequestUri.OriginalString), messageHandlerResult);

            var response = await "http://www.google.com".AsAGetRequest()
                .WithPathSegment("global")
                .WithPathSegment("http")
                .WithPathSegment("test")
                .ReadResponseAsString();
            Assert.Equal("response from a global client", response);
        }
    }
}
