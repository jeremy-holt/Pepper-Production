using System;
using System.Threading.Tasks;
using PCal.DataTransportWrappers;
using PCal.Extensions;
using PCal.Models;
using Raven.Client;

namespace PCal.Services
{
    public class BaseService
    {
        protected IAsyncDocumentSession Session { get; }

        protected BaseService(IAsyncDocumentSession session)
        {
            Session = session;
        }

        protected async Task<SaveModel> SaveAsync(IEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var updateMessage = entity.Id.IsNullOrEmpty() ? "Created" : "Updated";

            await Session.StoreAsync(entity);
            await Session.SaveChangesAsync();

            var entityName = entity.GetType().Name.CamelCaseToSpaces();
            var message = $"{updateMessage} {entityName} with Id = {entity.Id}";
            return new SaveModel(entity,message);
        }

        protected void CheckEntityWasFound(IEntity entity, string id)
        {          
            if (entity == null)
            {
                var entityName = entity.GetType().Name.CamelCaseToSpaces();
                var message = $"{entityName} with Id = {id} not found";

                throw new EntityNotFoundException(message);
            }
        }
    }
}
