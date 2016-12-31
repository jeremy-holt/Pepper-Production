using PCal.Services;
using PCal.Startup_config;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Config;
using Raven.Imports.Newtonsoft.Json;
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
            //var store= NewDocumentStore(configureStore: c => c.Configuration.Storage.Voron.AllowOn32Bits = true);
            var store = base.NewDocumentStore();

            store.Conventions = new DocumentConvention
            {
                IdentityPartsSeparator = "-"
            };

            store.Conventions.CustomizeJsonSerializer +=
                serializer => { serializer.ObjectCreationHandling = ObjectCreationHandling.Auto; };
            
            store.Initialize();

            return store;

        }

        protected static IFarmProductService GetFarmProductService(IAsyncDocumentSession session)
        {
            return new FarmProductService(session);
        }
    }
}