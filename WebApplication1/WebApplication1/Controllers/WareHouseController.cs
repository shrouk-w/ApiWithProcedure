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
    public async Task<IActionResult> AddProductAsync(RequestAddProductDTO product,CancellationToken cancellationToken)
    {
        var response = await _wareHouseService.AddProductAsync(product, cancellationToken);
        return Ok(response);
    }

    [HttpPut("procedure")]
    public async Task<IActionResult> AddProductProcedureAsync(RequestAddProductDTO product, CancellationToken cancellationToken)
    {
        return Ok(await _wareHouseService.AddProductViaProcedureAsync(product, cancellationToken));
    }
    
    
}
