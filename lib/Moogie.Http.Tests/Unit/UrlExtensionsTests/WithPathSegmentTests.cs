using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.UrlExtensionsTests
{
    public class WithPathSegmentTests : UnitTest
    {
        [Fact]
        public async Task Can_Add_Multiple_Path_Segments()
        {
            OnRequestMade(r => Assert.Contains("/one/three/two/ten", r.RequestUri.ToString()));

            await HttpRequest
                .WithPathSegment("one")
                .WithPathSegment("three")
                .WithPathSegment("two")
                .WithPathSegment("ten")
                .EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Can_Add_Path_Segments_And_Query_String_Parameters()
        {
            OnRequestMade(r => Assert.Contains("/one/three/two?a=1&b=2", r.RequestUri.ToString()));

            await HttpRequest
                .WithPathSegment("one")
                .WithQueryParameter("a", "1")
                .WithPathSegment("three")
                .WithPathSegment("two")
                .WithQueryParameter("b", "2")
                .EnsureSuccessStatusCode();
        }
    }
}
