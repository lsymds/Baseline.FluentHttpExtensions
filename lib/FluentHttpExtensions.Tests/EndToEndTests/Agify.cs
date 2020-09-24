using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable NotNullMemberIsNotInitialized
#pragma warning disable 8618

namespace FluentHttpExtensions.Tests.EndToEndTests
{
    public class Agify
    {
        [Fact]
        public async Task Can_Get_Approximate_Age()
        {
            var response = (await "https://api.agify.io"
                .AsAGetRequest()
                .WithQueryParameter("name", "michael")
                .WithQueryParameter("name", "jeff")
                .ReadJsonResponseAs<IEnumerable<Agez>>()).ToList();

            Assert.Equal(2, response.Count);

            Assert.Equal("michael", response[0].Name);
            Assert.Equal(68, response[0].Age);
            Assert.True(response[0].Count > 230000);

            Assert.Equal("jeff", response[1].Name);
            Assert.Equal(50, response[1].Age);
            Assert.True(response[0].Count > 30000);
        }
    }

    public class Agez
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("age")]
        public ushort Age { get; set; }

        [JsonPropertyName("count")]
        public long Count { get; set; }
    }


}
