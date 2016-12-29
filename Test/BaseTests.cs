using PCal.Startup_config;
using Raven.Client;
using Raven.Client.Document;
using Raven.Database.Config;
using Raven.Tests.Helpers;

namespace Test
{
    public class BaseTests : RavenTestBase
    {
        protected BaseTests()
        {
            AutoMapperConfiguration.CreateMapper();
        }


        protected IDocumentStore NewDocumentStore()
        {
            var store= NewDocumentStore(configureStore: c => c.Configuration.Storage.Voron.AllowOn32Bits = true);

            store.Conventions = new DocumentConvention
            {
                IdentityPartsSeparator = "-"
            };

            return store;

        }
    }
}