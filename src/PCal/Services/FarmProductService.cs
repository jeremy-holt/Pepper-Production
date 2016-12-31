using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PCal.Models;
using PCal.Responses;
using Raven.Client;
using Raven.Client.Linq;

namespace PCal.Services
{
    public interface IFarmProductService 
    {
        Task<FarmProduct> GetAsync(string id);
        Task<List<FarmProduct>> GetAsync();
        Task<SaveResponse> SaveAsync(FarmProduct entity);
        Task<DeleteResponse> DeleteAsync(string id);
    }

    public class FarmProductService : BaseService, IFarmProductService 
    {
        public FarmProductService(IAsyncDocumentSession session) : base(session)
        {
        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            return await DeleteAsync<FarmProduct>(id);
        }


        public async Task<List<FarmProduct>> GetAsync()
        {
            var query = await Session.Query<FarmProduct>().Take(1024).OrderBy(c => c.Name).ToListAsync();
            return query.ToList();
        }

        public async Task<FarmProduct> GetAsync(string id)
        {
            return await GetAsync<FarmProduct>(id);
        }

        public async Task<SaveResponse> SaveAsync(FarmProduct entity)
        {
            return await base.SaveAsync(entity);
        }

      
    }
}