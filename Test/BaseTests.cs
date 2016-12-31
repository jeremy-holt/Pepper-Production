using PCal.Services;
using PCal.Startup_config;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Tests.Helpers;

namespace Test
{
    public class BaseTests : RavenTestBase
    {
        protected BaseTests()
        {
            AutoMapperConfiguration.CreateMapper();
        }


        protected EmbeddableDocumentStore NewDocumentStore()
        {
            var store = NewDocumentStore(conventions: new DocumentConvention {IdentityPartsSeparator = "-"});

            store.Initialize();

            return store;
        }

        protected static IFarmProductService GetFarmProductService(IAsyncDocumentSession session)
        {
            return new FarmProductService(session);
        }
    }
}