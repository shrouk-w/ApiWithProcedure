using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IWareHouseService
{
    Task<int> AddProductAsync(RequestAddProductDTO product, CancellationToken cancellationToken);
    Task<int> AddProductViaProcedureAsync(RequestAddProductDTO product, CancellationToken cancellationToken);
}