using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PCal.Models;
using PCal.Services;
using Raven.Client;

namespace PCal.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ITestService _service;

        public ValuesController(ITestService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<County>> GetAsync()
        {
            var query = await _service.Session.Query<County>().ToListAsync();

            return query;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}