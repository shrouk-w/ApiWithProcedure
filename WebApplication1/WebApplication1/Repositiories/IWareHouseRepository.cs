using WebApplication1.DTOs;

namespace WebApplication1.Repositiories;

public interface IWareHouseRepository
{
    Task<int> AddProductAsync(AddProductToDB_DTO product, CancellationToken cancellationToken);

    Task<bool> DoesProductExistAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> DoesWareHouseExistAsync(int id, CancellationToken cancellationToken);
    
    Task<int> GetOrderIdAsync(int id,int amount,DateTime date, CancellationToken cancellationToken);
    
    Task<bool> DoesOrderExistAsync(int id, CancellationToken cancellationToken);
    
    Task<float> GetPriceAsync(int id, CancellationToken cancellationToken);
    
}