using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.Unit.UrlExtensionsTests
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
            OnRequestMade(r => Assert.Contains("?a=b&foo=bar&name=bill", r.RequestUri.ToString()));

            await HttpRequest
                .WithQueryParameter("a", "b")
                .WithQueryParameter("foo", "bar")
                .WithQueryParameter("name", "bill")
                .EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Can_Add_Same_Parameter_More_Than_Once()
        {
            OnRequestMade(r => Assert.Contains("?a=b&a=c", r.RequestUri.ToString()));

            await HttpRequest
                .WithQueryParameter("a", "b")
                .WithQueryParameter("a", "c")
                .EnsureSuccessStatusCode();
        }
    }
}
