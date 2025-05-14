using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Repositiories;

namespace WebApplication1.Services;

public class WareHouseService : IWareHouseService
{
    
    private readonly IWareHouseRepository _wareHouseRepository;

    public WareHouseService(IWareHouseRepository wareHouseRepository)
    {
        _wareHouseRepository = wareHouseRepository;
    }

    public async Task<int> AddProductAsync(RequestAddProductDTO product, CancellationToken cancellationToken)
    {
        
        if(! await _wareHouseRepository.DoesProductExistAsync(product.IdProduct,cancellationToken))
            throw new NotFoundException("Product doesnt exist");
        
        if(! await _wareHouseRepository.DoesWareHouseExistAsync(product.IdWarehouse,cancellationToken))
            throw new NotFoundException("Warehouse doesnt exist");
        
        var orderid = await _wareHouseRepository.GetOrderIdAsync(product.IdProduct,product.Amount,DateTime.Now,cancellationToken);
        
        if(orderid<=0)
            throw new NotFoundException("Order doesnt exist");
        
        if(await _wareHouseRepository.DoesOrderExistAsync(orderid, cancellationToken))
            throw new ConflictException("Order is already being proceeded with");

        var price = await _wareHouseRepository.GetPriceAsync(product.IdProduct, cancellationToken);
        if(price <= 0)
            throw new ConflictException("Price is lesser than 0");
        
        var argument = new AddProductToDB_DTO()
        {
            Price = price,
            IdOrder = orderid,
            IdProduct = product.IdProduct,
            IdWarehouse = product.IdWarehouse,
            Amount = product.Amount,
            Date = DateTime.Now
        };
        
        var response = await _wareHouseRepository.AddProductAsync(argument, cancellationToken);
        return response;
    }

    public async Task<int> AddProductViaProcedureAsync(RequestAddProductDTO product, CancellationToken cancellationToken)
    {
        var response = await _wareHouseRepository.AddProductViaProcedureAsync(product, cancellationToken);
        if(response<=0)
            throw new NotFoundException("Product doesnt exist");
        return response;
    }

}