using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.UrlExtensionsTests
{
    public class WithPathSegmentTests : UnitTest
    {
        [Fact]
        public async Task Can_Add_Multiple_Path_Segments()
        {
            var guid = Guid.NewGuid();
            OnRequestMade(r => r.RequestUri.ToString().Should().Contain($"/one/3/2/10/{guid}/18/25/{ulong.MaxValue}/fivebillion"));

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
                .EnsureSuccessStatusCodeAsync();
        }

        [Fact]
        public async Task Can_Add_Path_Segments_And_Query_String_Parameters()
        {
            OnRequestMade(r => r.RequestUri.ToString().Should().Contain("/one/three/two?a=1&b=2"));

            await HttpRequest
                .WithPathSegment("one")
                .WithQueryParameter("a", "1")
                .WithPathSegment("three")
                .WithPathSegment("two")
                .WithQueryParameter("b", "2")
                .EnsureSuccessStatusCodeAsync();
        }
    }
}
