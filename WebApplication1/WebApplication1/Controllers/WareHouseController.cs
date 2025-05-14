using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;



[ApiController]
[Route("api/[controller]")]
public class WareHouseController: ControllerBase
{
    
    private readonly IWareHouseService _wareHouseService;

    public WareHouseController(IWareHouseService wareHouseService)
    {
        _wareHouseService = wareHouseService;
    }

    [HttpPut]
    public async Task<IActionResult> AddProduct(AddProductDTO product)
    {
        var response = await _wareHouseService.AddProduct(product);
        return Ok(response);
    }
    
    
}
