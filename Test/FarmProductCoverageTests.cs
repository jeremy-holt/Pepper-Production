using FluentAssertions;
using PCal.Models;
using Xunit;

namespace Test
{
    public class FarmProductCoverageTests
    {
        [Fact]
        public void Should_be_able_to_initialize_a_FarmProductCoverage()
        {
            // Arrange
            var sut = new FarmProductCoverage(1, 100);

            // Assert
            sut.Year.Should().Be(1);
        }
    }
}