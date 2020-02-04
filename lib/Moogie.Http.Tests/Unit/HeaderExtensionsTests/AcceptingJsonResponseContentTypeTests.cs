using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.HeaderExtensionsTests
{
    public class AcceptingJsonContentTypeTests : UnitTest
    {
        [Fact]
        public async Task Accept_Header_Is_Set_To_Json_Content_Type()
        {
            OnRequestMade(r => Assert.Equal("application/json", r.Headers.Accept.ToString()));

            await MoogieHttpRequest
                .AsAGet()
                .AcceptingJsonResponseContentType()
                .EnsureResponseSuccessful();
        }

        [Fact]
        public async Task Accept_Header_Adds_Json_Content_Type_And_Doesnt_Replace_If_Replace_Is_False()
        {
            OnRequestMade(r => Assert.Equal("text/plain, application/json", r.Headers.Accept.ToString()));

            await MoogieHttpRequest
                .AcceptingResponseContentType("text/plain")
                .AcceptingJsonResponseContentType()
                .EnsureResponseSuccessful();
        }

        [Fact]
        public async Task Accept_Header_Replaces_With_Json_Content_Type_If_Replace_Is_True()
        {
            OnRequestMade(r => Assert.Equal("application/json", r.Headers.Accept.ToString()));

            await MoogieHttpRequest
                .AcceptingResponseContentType("text/plain")
                .AcceptingJsonResponseContentType(true)
                .EnsureResponseSuccessful();
        }
    }
}
