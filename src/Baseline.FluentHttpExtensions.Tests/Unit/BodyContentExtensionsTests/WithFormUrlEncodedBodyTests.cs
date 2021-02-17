using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.Tests.Unit.BodyContentExtensionsTests
{
    public class WithFormUrlEncodedBodyTests : UnitTest
    {
        [Fact]
        public void It_Throws_An_Exception_When_The_Body_Parameter_Is_Null()
        {
            Action act = () => HttpRequest.WithFormUrlEncodedBody(null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task Url_Encoded_Body_Is_Sent_Correctly()
        {
            var bodyContents = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "b"),
                new KeyValuePair<string, string>("a", "c"),
                new KeyValuePair<string, string>("anotherOne", "anotherOne"),
                new KeyValuePair<string, string>("aTrickyOne", "a=1,-!#")
            };

            OnRequestMade(handler =>
            {
                var content = handler.Content as FormUrlEncodedContent;
                content.Should().NotBeNull();
                content.ReadAsStringAsync().Result.Should().Be("a=b&a=c&anotherOne=anotherOne&aTrickyOne=a%3D1%2C-%21%23");
            });

            await HttpRequest.WithFormUrlEncodedBody(bodyContents.ToArray()).EnsureSuccessStatusCodeAsync();
        }
    }
}
