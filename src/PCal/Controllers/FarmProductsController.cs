using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Policy="DisneyUser")]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAsync();
            return Ok(data);
        }

        // GET api/farmProducts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var model = await _service.GetAsync(id);
            return Ok(model);
        }

        // POST api/farmProducts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FarmProduct farmProduct)
        {
            var data = await _service.SaveAsync(farmProduct);
            return Ok(data);
        }

        // PUT api/farmProducts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/farmProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await _service.DeleteAsync(id);
            return Ok(data);
        }
    }
}