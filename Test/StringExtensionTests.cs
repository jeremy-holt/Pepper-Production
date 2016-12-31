using System;
using FluentAssertions;
using PCal.Extensions;
using Xunit;

namespace Test
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("Products-1", "1")]
        [InlineData("Products-10", "10")]
        public void ToRavenId_should_return_number_part_of_id(string id, string expected)
        {
            // Arrange
            var actual = id.ToRavenId();
            actual.Should().Be(expected);
        }

        [Fact]
        public void ToRavenId_should_throw_exception_if_cant_extract_number_from_id()
        {
            // Arrange
            Action action = () => "Products".ToRavenId();
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("Unable to extract id number from Products");
        }
    }
}