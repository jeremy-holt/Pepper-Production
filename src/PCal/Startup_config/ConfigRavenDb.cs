using System.Reflection;
using Microsoft.Extensions.Options;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Imports.Newtonsoft.Json;

namespace PCal.Startup_config
{
    public class ConfigRavenDb
    {
        private readonly ApplicationOptions _optionsAccessor;

        public ConfigRavenDb(IOptions<ApplicationOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor.Value;
        }

        public IDocumentStore InitializeRavenDb(params Assembly[] assemblies)
        {
            var documentStore = new DocumentStore
            {
                Url = _optionsAccessor.Url,
                DefaultDatabase = _optionsAccessor.Database
                //Conventions = new DocumentConvention {IdentityPartsSeparator = "-'"}
            };

            documentStore.Conventions.CustomizeJsonSerializer +=
                serializer => { serializer.ObjectCreationHandling = ObjectCreationHandling.Auto; };
            documentStore.Initialize();

            assemblies.ForEach(assembly => IndexCreation.CreateIndexes(assembly, documentStore));

            return documentStore;
        }
    }
}