using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Moogie.Http.Tests.EndToEndTests
{
    public class CatFacts
    {
        [Fact]
        public async Task Can_Get_Cat_Facts()
        {
            var response = await "https://cat-fact.herokuapp.com"
                .AsAGetRequest()
                .WithPathSegment("facts")
                .ReadResponseAsString();

            Assert.False(string.IsNullOrWhiteSpace(response));
        }

        [Fact]
        public async Task Can_Get_Cat_Facts_As_Json()
        {
            var response = await new HttpRequest("https://cat-fact.herokuapp.com")
                .AsAGetRequest()
                .WithPathSegment("facts")
                .ReadJsonResponseAs<CatFactsObj>();

            Assert.NotEmpty(response.Facts);
            Assert.NotEmpty(response.Facts.First().Id);
            Assert.NotEmpty(response.Facts.First().Text);
            Assert.NotEmpty(response.Facts.First().Type);
            Assert.True(response.Facts.First().Upvotes >= 0);
        }
    }

    public class CatFactsObj
    {
        public class CatFact
        {
            [JsonPropertyName("_id")]
            public string Id { get; set; }

            [JsonPropertyName("text")]
            public string Text { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("upvotes")]
            public int Upvotes { get; set; }
        }

        [JsonPropertyName("all")]
        public IEnumerable<CatFact> Facts { get; set; } = null!;

    }
}
