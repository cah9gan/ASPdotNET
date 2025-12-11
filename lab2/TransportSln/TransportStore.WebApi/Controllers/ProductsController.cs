using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportStore.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace TransportStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IStoreRepository _repository;

        public ProductsController(IStoreRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Transport> GetTransports()
        {
            return _repository.Transports;
        }

        [HttpGet("{id}")]
        public ActionResult<Transport> GetTransport(long id)
        {
            var transport = _repository.Transports.FirstOrDefault(p => p.Id == id);

            if (transport == null)
            {
                return NotFound();
            }

            return transport;
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Transport> CreateTransport([FromBody] Transport transport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.SaveTransport(transport);
            return CreatedAtAction(nameof(GetTransport), new { id = transport.Id }, transport);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateTransport(long id, [FromBody] Transport transport)
        {
            if (id != transport.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = _repository.Transports.FirstOrDefault(t => t.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            _repository.SaveTransport(transport);
            return NoContent(); 
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteTransport(long id)
        {
            var transport = _repository.Transports.FirstOrDefault(p => p.Id == id);

            if (transport == null)
            {
                return NotFound();
            }

            _repository.DeleteTransport(transport);

            return Ok(transport);
        }
    }
}