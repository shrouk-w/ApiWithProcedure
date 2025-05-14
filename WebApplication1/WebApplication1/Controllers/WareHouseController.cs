using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers;



[ApiController]
[Route("api/[controller]")]
public class WareHouseController: ControllerBase
{
    
    private readonly ITripService _tripService;

    public WareHouseController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpPut]
    public async Task<IActionResult> AddProduct(AddProductDTO product)
    {
        var response = await _tripService.AddProduct(product);
        return Ok();
    }
    
    
}
