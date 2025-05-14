using WebApplication1.DTOs;

namespace WebApplication1.Repositiories;

public interface IWareHouseRepository
{
    Task<int> AddProduct(AddProductDTO product);
}