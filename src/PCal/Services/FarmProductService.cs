using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PCal.DataTransportWrappers;
using PCal.Extensions;
using PCal.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace PCal.Services
{
    public interface IFarmProductService
    {
        Task<FarmProduct> GetFarmProduct(string id);
        Task<List<FarmProduct>> GetFarmProductsAsync();
        Task<SaveModel> SaveAsync(FarmProduct entity);
        Task<DeleteModel> DeleteAsync(string id);
    }

    public class FarmProductService : BaseService, IFarmProductService
    {
        public FarmProductService(IAsyncDocumentSession session) : base(session)
        {
        }

        public async Task<DeleteModel> DeleteAsync(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var entity = await Session.LoadAsync<FarmProduct>(id);
            var ravenId = id.ToRavenId();

            if (entity == null)
                throw new EntityNotFoundException($"Farm Product Id = {ravenId} does not exist");
            Session.Delete(entity);
            await Session.SaveChangesAsync();

            return new DeleteModel($"Deleted Farm Product with Id = {ravenId}");
        }

        public async Task<List<FarmProduct>> GetFarmProductsAsync()
        {
            var query = await Session.Query<FarmProduct>().Take(1024).OrderBy(c => c.Name).ToListAsync();
            return query.ToList();
        }

        public async Task<FarmProduct> GetFarmProduct(string id)
        {
            var result = await Session.LoadAsync<FarmProduct>(id);
            if (result == null)
                throw new EntityNotFoundException($"Farm Product with ID = {id} not found");

            return result;
        }

        public async Task<SaveModel> SaveAsync(FarmProduct entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var updateMessage = entity.Id.IsNullOrEmpty() ? "Created" : "Updated";

            await Session.StoreAsync(entity);
            await Session.SaveChangesAsync();
            return new SaveModel(entity, $"{updateMessage} Farm Product with Id = {entity.Id}");
        }
    }
}