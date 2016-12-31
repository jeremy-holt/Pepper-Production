using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PCal.DataTransportWrappers;
using PCal.Models;
using PCal.Services;

namespace PCal.Controllers
{
    [Route("api/[controller]")]
    public class FarmProductsController : Controller
    {
        private readonly IFarmProductService _service;
        // GET api/farmProducts
        public FarmProductsController(IFarmProductService service)
        {
            _service = service;
            Init();
        }

        private void Init()
        {
            var farmProduct = new FarmProduct("NPK", CoverageType.LitresPerHectare);
            farmProduct.AddCoverage(1, 15);
            farmProduct.AddCoverage(2, 10);
            farmProduct.AddCoverage(3, 10);

            _service.SaveAsync(farmProduct).Wait();
        }

        [HttpGet]
        public async Task<List<FarmProduct>> Get()
        {
            return await _service.GetFarmProductsAsync();
        }

        // GET api/farmProducts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var model = await _service.GetFarmProduct(id);
            return Ok(model);
        }

        // POST api/farmProducts
        [HttpPost]
        public async Task<SaveModel> Post([FromBody] FarmProduct farmProduct)
        {
            return await _service.SaveAsync(farmProduct);
        }

        // PUT api/farmProducts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/farmProducts/5
        [HttpDelete("{id}")]
        public async Task<DeleteModel> Delete(string id)
        {
            return await _service.DeleteAsync(id);
        }
    }
}