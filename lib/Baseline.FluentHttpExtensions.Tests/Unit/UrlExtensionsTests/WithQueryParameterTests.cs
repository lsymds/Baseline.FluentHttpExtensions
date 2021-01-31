using System;
using System.Threading.Tasks;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.UrlExtensionsTests
{
    public class WithQueryParameterTests : UnitTest
    {
        [Fact]
        public async Task Single_Query_Parameter_Can_Be_Added()
        {
            OnRequestMade(r => Assert.Contains("?a=b", r.RequestUri.ToString()));

            await HttpRequest
                .WithQueryParameter("a", "b")
                .EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Multiple_Query_Parameters_Can_Be_Added()
        {
            var guid = Guid.NewGuid();

            OnRequestMade(r => Assert.Contains(
                $"?a=b&foo=bar&name=bill&fin=0&fon=1&far=10&fun=32&bingo={guid}&bar=3&bus={ulong.MaxValue}",
                r.RequestUri.ToString()));

            await HttpRequest
                .WithQueryParameter("a", "b")
                .WithQueryParameter("foo", "bar")
                .WithQueryParameter("name", "bill")
                .WithQueryParameter("fin", 0)
                .WithQueryParameter("fon", (uint)1)
                .WithQueryParameter("far", (ushort)10)
                .WithQueryParameter("fun", (long)32)
                .WithQueryParameter("bingo", guid)
                .WithQueryParameter("bar", (short)3)
                .WithQueryParameter("bus", ulong.MaxValue)
                .EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Can_Add_Same_Parameter_More_Than_Once()
        {
            OnRequestMade(r => Assert.Contains("?a=b&a=c&c=5&c=55", r.RequestUri.ToString()));

            await HttpRequest
                .WithQueryParameter("a", "b")
                .WithQueryParameter("a", "c")
                .WithQueryParameter("c", 5)
                .WithQueryParameter("c", (ushort)55)
                .EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Invalid_Or_Empty_Query_String_Value_Is_Not_Added()
        {
            OnRequestMade(r => Assert.Contains("?b=1&d=3", r.RequestUri.ToString()));

            await HttpRequest
                .WithQueryParameter("a", null)
                .WithQueryParameter("b", "1")
                .WithQueryParameter("c", " ")
                .WithQueryParameter("d", "3")
                .WithQueryParameter("e", string.Empty)
                .EnsureSuccessStatusCode();
        }
    }
}
