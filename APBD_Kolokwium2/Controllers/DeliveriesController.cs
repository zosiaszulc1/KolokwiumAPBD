using APBD_Kolokwium2.DTOs;
using APBD_Kolokwium2.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_KolokwiumPoprawa.Controllers;

[ApiController]
[Route("api/deliveries")]
public class DeliveriesController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    public DeliveriesController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDelivery(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Delivery id must be greater than 0." });
        }

        var delivery = await _deliveryService.GetDeliveryAsync(id);

        if (delivery == null)
        {
            return NotFound(new { message = $"Delivery with id {id} was not found." });
        }

        return Ok(delivery);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _deliveryService.CreateDeliveryAsync(request);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, new { message = result.Message });
        }

        return Created($"/api/deliveries/{result.DeliveryId}", new
        {
            message = result.Message,
            deliveryId = result.DeliveryId
        });
    }
}