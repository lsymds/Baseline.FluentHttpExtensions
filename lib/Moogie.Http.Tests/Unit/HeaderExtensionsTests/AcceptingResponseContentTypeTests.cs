using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.HeaderExtensionsTests
{
    public class AcceptingResponseContentTypeTests : UnitTest
    {
        [Fact]
        public async Task Accept_Header_Is_Set_To_Passed_In_Response_Type()
        {
            OnRequestMade(r => Assert.Equal("text/plain", r.Headers.Accept.ToString()));

            await MoogieHttpRequest
                .AsAGet()
                .AcceptingResponseContentType("text/plain")
                .EnsureResponseSuccessful();
        }

        [Fact]
        public async Task Accept_Header_Adds_And_Doesnt_Replace_If_Replace_Is_False()
        {
            OnRequestMade(r => Assert.Equal("text/plain, application/json", r.Headers.Accept.ToString()));

            await MoogieHttpRequest
                .AcceptingResponseContentType("text/plain")
                .AcceptingResponseContentType("application/json")
                .EnsureResponseSuccessful();
        }

        [Fact]
        public async Task Accept_Header_Replaces_If_Replace_Is_True()
        {
            OnRequestMade(r => Assert.Equal("application/json", r.Headers.Accept.ToString()));

            await MoogieHttpRequest
                .AcceptingResponseContentType("text/plain")
                .AcceptingResponseContentType("application/json", true)
                .EnsureResponseSuccessful();
        }
    }
}
