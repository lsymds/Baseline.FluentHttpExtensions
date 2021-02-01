using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.FluentHttpExtensions.CombinedFileTests
{
    public class CombinedFileJsonPlaceholderTests
    {
        [Fact]
        public async Task It_Can_Retrieve_A_User()
        {
            var user = await "https://jsonplaceholder.typicode.com/users/1"
                .AsAGetRequest()
                .ReadJsonResponseAsAsync<User>();

            user.Id.Should().Be(1);
            user.Name.Should().Be("Leanne Graham");
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
