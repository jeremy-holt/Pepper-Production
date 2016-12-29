using System.Threading.Tasks;
using FluentAssertions;
using PCal.Models;
using PCal.Services;
using Raven.Client;
using Xunit;

namespace Test
{
    public class FarmProductServiceTests : BaseTests
    {
        private IFarmProductService GetFarmProductService(IAsyncDocumentSession session)
        {
            return new FarmProductService(session);
        }

        [Fact]
        public async Task GetFarmProduct_should_load_a_previously_saved_entity()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var farmProduct = new FarmProduct("NPK", CoverageType.LitresPerHectare);
                    await session.StoreAsync(farmProduct);
                    await session.SaveChangesAsync();
                }

                // Act
                using (var session = store.OpenAsyncSession())
                {
                    var service = GetFarmProductService(session);
                    var actual = await service.GetFarmProduct("FarmProducts/1");

                    // Assert
                    actual.Name.Should().Be("NPK");
                    actual.CoverageType.Should().Be(CoverageType.LitresPerHectare);
                }
            }
        }

        [Fact]
        public async Task SaveAsync_should_save_a_farm_product()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var farmProduct = new FarmProduct(null, "NPK", CoverageType.GramsPerPlant);
                    var sut = GetFarmProductService(session);

                    // Act
                    var model = await sut.SaveAsync(farmProduct);
                    var actual = await session.LoadAsync<FarmProduct>("FarmProducts/1");

                    // Assert
                    actual.Name.Should().Be("NPK");
                    actual.CoverageType.Should().Be(CoverageType.GramsPerPlant);
                    model.Message.Should().Be("Saved Farm Product with Id = FarmProducts/1");
                }
            }
        }

        [Fact]
        public async Task SaveAsync_should_update_an_existing_entity()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var farmProduct = new FarmProduct("NPK", CoverageType.GramsPerPlant);
                    farmProduct.AddCoverage(1,100);

                    await session.StoreAsync(farmProduct);
                    await session.SaveChangesAsync();

                    // Act
                    var service = GetFarmProductService(session);
                    var actual = await service.GetFarmProduct("FarmProducts/1");
                    actual.Name = "Updated";
                    var model = await service.SaveAsync(actual);
                    model.Entity.Id.Should().Be("FarmProducts/1");
                }

                using (var session = store.OpenAsyncSession())
                {
                    // Assert
                    var service = GetFarmProductService(session);
                    var actual = await service.GetFarmProduct("FarmProducts/1");

                    actual.Name.Should().Be("Updated");
                    actual.Coverages.Should().HaveCount(1);
                }
            }
        }
    }
}