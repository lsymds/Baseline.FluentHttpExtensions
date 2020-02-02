using Xunit;

namespace Moogie.Http.Tests.Unit.GeneralExtensionsTests
{
    public class ShouldAllowAutoRedirectTests
    {
        [Fact]
        public void ShouldAllowAutoRedirect_Is_Set_To_False_Initially()
        {
            // Arrange & Act.
            var request = new MoogieHttpRequest("http://www.google.com");

            // Assert.
            Assert.False(request.AllowAutoRedirect);
        }

        [Fact]
        public void Can_Set_AllowAutoRedirect()
        {
            // Arrange & Act.
            var request = new MoogieHttpRequest("http://www.google.com")
                .ShouldAllowAutoRedirect(true);

            // Assert.
            Assert.True(request.AllowAutoRedirect);
        }
    }
}
