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

    public class FarmProductTests
    {
        [Fact]
        public void AddCoverage_should_add_a_coverage_to_the_FarmProduct()
        {
            // Arrange
            var sut = new FarmProduct("123", "Test", CoverageType.KgPerHectare);
            const int year = 1;
            const double quantity = 100;

            // Act
            sut.AddCoverage(year, quantity);

            // Assert
            sut.Coverages.Should().HaveCount(1);
            sut.Coverages[0].Year.Should().Be(1);
            sut.Coverages[0].Quantity.Should().Be(100);
        }

        [Fact]
        public void CoverageText_should_return_formatted_text_for_coverage_with_single_year()
        {
            // Arrange
            var sut = new FarmProduct("123", "Test", CoverageType.LitresPerHectare);
            sut.AddCoverage(1, 55);

            // Act
            var actual = sut.CoverageText;

            // Assert
            actual.Should().Be("Year 1 and onwards: 55 lt/ha");
        }

        [Fact]
        public void CoverageText_should_return_formatted_text_for_coverages_with_multiple_years()
        {
            // Arrange
            var sut = new FarmProduct("123", "Test", CoverageType.KgPerHectare);
            sut.AddCoverage(1, 500);
            sut.AddCoverage(2, 750);
            sut.AddCoverage(3, 1000);

            // Act
            var actual = sut.CoverageText;

            // Assert
            actual.Should().Be("Year 1: 500 kg/ha, Year 2: 750 kg/ha, Year 3 and onwards: 1000 kg/ha");
        }

        [Fact]
        public void GetCoverageText_should_return_empty_string_for_no_coverages()
        {
            // Arrange
            var sut = new FarmProduct("123", "Test", CoverageType.KgPerHectare);

            // Assert
            sut.Coverages.Should().HaveCount(0);
        }
    }
}