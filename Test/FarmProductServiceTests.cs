using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PCal.DataTransportWrappers;
using PCal.Models;
using PCal.Services;
using Ploeh.AutoFixture;
using Raven.Client;
using Xunit;

namespace Test
{
    public class FarmProductServiceTests : BaseTests
    {
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
                    model.Message.Should().Be("Created Farm Product with Id = FarmProducts/1");
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

        [Fact]
        public async Task SaveAsync_should_only_save_the_entity_once()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var farmProduct = new FarmProduct("NPK",CoverageType.GramsPerPlant);
                    var service = GetFarmProductService(session);

                    // Act
                    farmProduct.Id.Should().BeNullOrEmpty();

                    var model1 = await service.SaveAsync(farmProduct);
                    model1.Entity.Id.Should().Be("FarmProducts/1");
                    model1.Message.Should().StartWith("Created");

                    var model2 = await service.SaveAsync(farmProduct);
                    model2.Entity.Id.Should().Be("FarmProducts/1");
                    model2.Message.Should().StartWith("Updated");

                    var model3 = await service.SaveAsync(farmProduct);
                    model3.Entity.Id.Should().Be("FarmProducts/1");
                    model3.Message.Should().StartWith("Updated");
                }
            }
        }

        [Fact]
        public async Task GetFarmProducts_should_return_list_of_farm_products()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var fixture = new Fixture();
                    var products = fixture.CreateMany<FarmProduct>().ToList();
                    var service = GetFarmProductService(session);
                    
                    foreach (var c in products)
                    {
                        await service.SaveAsync(c);
                    }

                    // Act
                    var list = await service.GetFarmProductsAsync();

                    // Assert
                    list.Should().HaveCount(3)
                        .And.BeInAscendingOrder(x => x.Name);


                }
            }
        }

        [Fact]
        public async Task Delete_should_delete_a_FarmProduct()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var fixture = new Fixture();
                    var product = fixture.Build<FarmProduct>().Without(c=>c.Id).Create();
                    
                    var service = GetFarmProductService(session);
                    await service.SaveAsync(product);

                    // Act
                    DeleteModel model = await service.DeleteAsync("FarmProducts/1");

                    // Assert
                    model.Message.Should().Be("Deleted Farm Product with Id = 1");
                }
            }
        }
     }
}