using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IWareHouseService
{
    Task<int> AddProduct(AddProductDTO product);
}