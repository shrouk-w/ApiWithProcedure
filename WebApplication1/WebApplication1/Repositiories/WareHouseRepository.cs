using WebApplication1.DTOs;

namespace WebApplication1.Repositiories;

public class WareHouseRepository:IWareHouseRepository
{
    
    private readonly string _connectionString;
    
    public WareHouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<int> AddProduct(AddProductDTO product)
    {
        return 1;
    }
}