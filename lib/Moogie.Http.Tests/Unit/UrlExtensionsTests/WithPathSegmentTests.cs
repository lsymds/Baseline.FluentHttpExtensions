using System;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.UrlExtensionsTests
{
    public class WithPathSegmentTests : UnitTest
    {
        [Fact]
        public async Task Can_Add_Multiple_Path_Segments()
        {
            var guid = Guid.NewGuid();
            OnRequestMade(r => Assert.Contains($"/one/3/2/10/{guid}/18/25/{ulong.MaxValue}/fivebillion", r.RequestUri.ToString()));

            await HttpRequest
                .WithPathSegment("one")
                .WithPathSegment(3)
                .WithPathSegment((ushort)2)
                .WithPathSegment((short)10)
                .WithPathSegment(guid)
                .WithPathSegment((uint)18)
                .WithPathSegment((long)25)
                .WithPathSegment(ulong.MaxValue)
                .WithPathSegment("fivebillion")
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
