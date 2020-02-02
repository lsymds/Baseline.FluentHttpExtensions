using Xunit;

namespace Moogie.Http.Tests.Unit.GeneralExtensionsTests
{
    public class WithUserAgentTests
    {
        [Fact]
        public void User_Agent_Is_Set_In_MoogieHttpRequest_Object()
        {
            // Arrange & Act.
            var request = new MoogieHttpRequest("http://www.google.com")
                .WithUserAgent("moogie-http");

            // Assert.
            Assert.Equal("moogie-http", request.UserAgent);
        }
    }
}
