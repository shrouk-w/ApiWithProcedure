using WebApplication1.DTOs;
using WebApplication1.Repositiories;

namespace WebApplication1.Services;

public class WareHouseService : IWareHouseService
{
    
    private readonly IWareHouseRepository _wareHouseRepository;

    public WareHouseService(IWareHouseRepository wareHouseRepository)
    {
        _wareHouseRepository = wareHouseRepository;
    }

    public async Task<int> AddProduct(AddProductDTO product)
    {
        var response = await _wareHouseRepository.AddProduct(product);
        return response;
    }
}