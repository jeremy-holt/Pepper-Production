using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using PCal;
using PCal.Models;
using Ploeh.AutoFixture;
using Xunit;

namespace Test
{
    public class FarmProductServiceTests : BaseTests
    {
        [Fact]
        public async Task Delete_should_delete_a_FarmProduct()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var fixture = new Fixture();
                    var product = fixture.Build<FarmProduct>().Without(c => c.Id).Create();

                    var service = GetFarmProductService(session);
                    await service.SaveAsync(product);

                    // Act
                    var model = await service.DeleteAsync("FarmProducts-1");

                    // Assert
                    model.Message.Should().Be("Deleted Farm Product with Id:1");
                }
            }
        }

        [Fact]
        public async Task Delete_should_throw_exception_if_id_does_not_exist()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var service = GetFarmProductService(session);

                    // Act
                    Func<Task> action = async () =>
                    {
                        await service.DeleteAsync("FarmProducts-999");
                    };

                    // Assert
                    action.ShouldThrow<EntityNotFoundException>()
                        .WithMessage("Farm product Id:999 does not exist");

                    
                }
            }
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
                    var actual = await service.GetAsync("FarmProducts-1");

                    // Assert
                    actual.Name.Should().Be("NPK");
                    actual.CoverageType.Should().Be(CoverageType.LitresPerHectare);
                }
            }
        }

        [Fact]
        public async Task GetFarmProduct_should_return_not_found_error_message()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var service = GetFarmProductService(session);

                    // Act
                    Func<Task> action = async ()=>await service.GetAsync("FarmProducts-999");

                    // Assert
                    action.ShouldThrow<EntityNotFoundException>().WithMessage("Farm Product with Id:999 not found");
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
                        await service.SaveAsync(c);

                    // Act
                    var list = await service.GetAsync();

                    // Assert
                    list.Should().HaveCount(3)
                        .And.BeInAscendingOrder(x => x.Name);
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
                    const string FARM_PRODUCTS_ID_1 = "FarmProducts-1";
                    var farmProduct = new FarmProduct("NPK", CoverageType.GramsPerPlant);
                    var service = GetFarmProductService(session);

                    // Act
                    farmProduct.Id.Should().BeNullOrEmpty();

                    var model1 = await service.SaveAsync(farmProduct);
                    model1.Entity.Id.Should().Be(FARM_PRODUCTS_ID_1);
                    model1.Message.Should().StartWith("Created");

                    var model2 = await service.SaveAsync(farmProduct);
                    model2.Entity.Id.Should().Be(FARM_PRODUCTS_ID_1);
                    model2.Message.Should().StartWith("Updated");

                    var model3 = await service.SaveAsync(farmProduct);
                    model3.Entity.Id.Should().Be(FARM_PRODUCTS_ID_1);
                    model3.Message.Should().StartWith("Updated");
                }
            }
        }

        [Fact]
        public async Task SaveAsync_should_save_a_farm_product()
        {
            const string FARM_PRODUCTS_ID_1 = "FarmProducts-1";

            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var farmProduct = new FarmProduct(null, "NPK", CoverageType.GramsPerPlant);
                    var sut = GetFarmProductService(session);

                    // Act
                    var model = await sut.SaveAsync(farmProduct);

                    // Assert
                    var actual = await session.LoadAsync<FarmProduct>(FARM_PRODUCTS_ID_1);
                    actual.Name.Should().Be("NPK");
                    actual.CoverageType.Should().Be(CoverageType.GramsPerPlant);
                    model.Message.Should().Be("Created Farm Product with Id:1");
                }
            }
        }

        [Fact]
        public async Task SaveAsync_should_update_an_existing_entity()
        {
            const string FARM_PRODUCTS_ID_1 = "FarmProducts-1";

            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    // Arrange
                    var farmProduct = new FarmProduct("NPK", CoverageType.GramsPerPlant);
                    farmProduct.AddCoverage(1, 100);

                    await session.StoreAsync(farmProduct);
                    await session.SaveChangesAsync();

                    // Act
                    var service = GetFarmProductService(session);
                    var actual = await service.GetAsync(FARM_PRODUCTS_ID_1);
                    actual.Name = "Updated";
                    var model = await service.SaveAsync(actual);
                    model.Entity.Id.Should().Be(FARM_PRODUCTS_ID_1);
                }

                using (var session = store.OpenAsyncSession())
                {
                    // Assert
                    var service = GetFarmProductService(session);
                    var actual = await service.GetAsync(FARM_PRODUCTS_ID_1);

                    actual.Name.Should().Be("Updated");
                    actual.Coverages.Should().HaveCount(1);
                }
            }
        }
    }
}