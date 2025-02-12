using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebEmployees;
using System;
using WebApiModel;

namespace WebEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private ISuppliers repo;

        public SuppliersController(ISuppliers repo)
        {
            this.repo = repo;
        }

        // GET: api/suppliers
        // GET: api/suppliers/?country=[country]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Supplier>))]
        public async Task<IEnumerable<Supplier>> GetSuppliers(string? country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                return await repo.GetAllAsync();
            }
            else
            {
                return (await repo.GetAllAsync())
                    .Where(customer => customer.Country == country);
            }
        }

        // GET: api/suppliers/[id]
        [HttpGet("{id}", Name = nameof(GetSuppliers))]
        [ProducesResponseType(200, Type = typeof(Supplier))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSuppliers(int id)
        {
            Supplier c = await repo.GetAsync(id);
            if (c == null)
            {
                return NotFound();
            }

            return Ok(c);
        }

        // POST: api/suppliers
        // BODY: Supplier (JSON, XML)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Supplier))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Supplier c)
        {
            if (c == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Supplier added = await repo.CreateAsync(c);
            return CreatedAtRoute(
                routeName: nameof(GetSuppliers),
                routeValues: new { id = added.SupplierId },
                value: added);
        }

        // PUT: api/customers/[id]
        // BODY: Customer (JSON, XML)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] Supplier c)
        {
            if (c == null || c.SupplierId != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await repo.GetAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await repo.UpdateAsync(id, c);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await repo.GetAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            bool? deleted = await repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value)
            {
                return new NoContentResult();
            }
            else
            {
                return BadRequest(
                    $"Customer {id} was found but failed to delete.");
            }
        }
    }
}