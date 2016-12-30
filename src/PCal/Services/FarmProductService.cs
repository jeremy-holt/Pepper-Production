using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PCal.Extensions;
using PCal.Models;
using Raven.Client;

namespace PCal.Services
{
    public interface IFarmProductService
    {
        Task<FarmProduct> GetFarmProduct(string id);
        Task<List<FarmProduct>> GetFarmProduct();
        Task<FarmProductModel> SaveAsync(FarmProduct entity);
        Task Delete(string id);
    }

    public class FarmProductService : BaseService, IFarmProductService
    {
        public FarmProductService(IAsyncDocumentSession session) : base(session)
        {
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<FarmProduct>> GetFarmProduct()
        {
            throw new NotImplementedException();
        }

        public async Task<FarmProduct> GetFarmProduct(string id)
        {
            return await Session.LoadAsync<FarmProduct>(id);
        }

        public async Task<FarmProductModel> SaveAsync(FarmProduct entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var updateMessage = entity.Id.IsNullOrEmpty() ? "Created" : "Updated";

            await Session.StoreAsync(entity);
            await Session.SaveChangesAsync();
            return new FarmProductModel(entity, $"{updateMessage} Farm Product with Id = {entity.Id}");
        }
    }

    public class FarmProductModel
    {
        public IEntity Entity { get; }
        public string Message { get; }

        public FarmProductModel(IEntity entity, string message)
        {
            Entity = entity;
            Message = message;
        }
    }
}