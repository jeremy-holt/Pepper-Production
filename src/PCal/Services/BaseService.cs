using System;
using System.Threading.Tasks;
using PCal.Extensions;
using PCal.Models;
using PCal.Responses;
using Raven.Client;

namespace PCal.Services
{
    public class BaseService:ServiceBase
    {
        protected BaseService(IAsyncDocumentSession session):base(session)
        {            
        }       

        protected async Task<SaveResponse> SaveAsync(IEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var updateMessage = entity.Id.IsNullOrEmpty() ? "Created" : "Updated";

            await Session.StoreAsync(entity);
            await Session.SaveChangesAsync();
            
            var message = $"{updateMessage} {entity.GetType().Name.CamelCaseToSpaces()} with Id = {entity.Id}";

            return new SaveResponse(entity, message);
        }

        protected async Task<T> GetAsync<T>(string id) where T : IEntity
        {
            var entity = await Session.LoadAsync<T>(id);

            if (entity != null)
            {
                return entity;
            }

            var entityName = typeof(T).Name.CamelCaseToSpaces();            
            var message = $"{entityName} with Id = {id} not found";

            throw new EntityNotFoundException(message);
        }

        protected async Task<DeleteResponse> DeleteAsync<T>(string id) where T : IEntity
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var ravenId = id.ToRavenId();

            var entity = await Session.LoadAsync<T>(id);

            if (entity == null)
                throw new EntityNotFoundException($"Farm Product Id = {ravenId} does not exist");

            Session.Delete(entity);
            await Session.SaveChangesAsync();

            return new DeleteResponse($"Deleted Farm Product with Id = {ravenId}");
        }
    }
}