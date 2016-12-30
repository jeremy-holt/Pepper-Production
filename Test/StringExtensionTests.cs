using FluentAssertions;
using PCal.Extensions;
using Xunit;

namespace Test
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("Products/1", "1")]
        [InlineData("Products/10", "10")]        
        [InlineData("Products", null)]        
        public void ToRavenId_should_return_number_part_of_id(string id, string expected)
        {
            // Arrange
            var actual = id.ToRavenId();
            actual.Should().Be(expected);
        }

        
    }
}