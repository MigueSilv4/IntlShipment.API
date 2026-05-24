using IntlShipment.Helpers;
using IntlShipment.Models;
using IntlShipment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntlShipment.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<Shipment>>> Create([FromBody] Shipment shipment)
        {
            var response = await _shipmentService.Create(shipment);
            if (response.Success)
                return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
            else
                return BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<Response<PaginatedList<Shipment>>>> GetAll(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? estado = null)
        {
            var response = await _shipmentService.GetAll(pageIndex, pageSize, estado);
            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Shipment>>> GetById(int id)
        {
            var response = await _shipmentService.GetById(id);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<Shipment>>> Update(int id, [FromBody] Shipment shipment)
        {
            var response = await _shipmentService.Update(id, shipment);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }

        [HttpPatch("{id}/estado")]
        public async Task<ActionResult<Response<Shipment>>> UpdateState(int id, [FromQuery] string estado)
        {
            var response = await _shipmentService.UpdateState(id, estado);
            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }


        [HttpPatch("{id}/cancelar")]
        public async Task<ActionResult<Response<Shipment>>> Cancel(int id)
        {
            var response = await _shipmentService.Cancel(id);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<Shipment>>> Delete(int id)
        {
            var response = await _shipmentService.Delete(id);
            if (response.Success)
                return Ok(response);
            else
                return NotFound(response);
        }
    }
}
