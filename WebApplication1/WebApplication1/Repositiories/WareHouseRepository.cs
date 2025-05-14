using Microsoft.Data.SqlClient;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;

namespace WebApplication1.Repositiories;

public class WareHouseRepository:IWareHouseRepository
{
    
    private readonly string _connectionString;
    
    public WareHouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<int> AddProductAsync(AddProductToDB_DTO product, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            //--------- updateing fulfill date
            var query1 = @"UPDATE [dbo].[Order]
                             SET [FulfilledAt] = @date";
            await using var checkCommand = new SqlCommand(query1, connection, (SqlTransaction)transaction);
            checkCommand.Parameters.AddWithValue("@date", product.Date);

            var updated = await checkCommand.ExecuteNonQueryAsync(cancellationToken);
            
            if(updated <= 0)
                throw new Exception("Update failed");

            
            //----------- insert into product-warehouse
            
            
            var insertQuery = @"INSERT INTO [dbo].[Product_Warehouse] (IdWarehouse,IdProduct, IdOrder,Amount,Price, CreatedAt)
                            VALUES (@id_warehouse, @id_product, @id_order, @amount, @price, @date);
                            SELECT SCOPE_IDENTITY();";

            await using var insertCommand = new SqlCommand(insertQuery, connection, (SqlTransaction)transaction);
            insertCommand.Parameters.AddWithValue("@id_product", product.IdProduct);
            insertCommand.Parameters.AddWithValue("@id_warehouse", product.IdWarehouse);
            insertCommand.Parameters.AddWithValue("@id_order", product.IdOrder);
            insertCommand.Parameters.AddWithValue("@amount", product.Amount);
            insertCommand.Parameters.AddWithValue("@price", product.Price*product.Amount);
            insertCommand.Parameters.AddWithValue("@date", product.Date);

            var insertedIdObj = await insertCommand.ExecuteScalarAsync(cancellationToken);
            int insertedId = Convert.ToInt32(insertedIdObj);

            
            
            await transaction.CommitAsync(cancellationToken);

            return insertedId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new Exception($"Update failed");
        }
    }
    
    public async Task<bool> DoesProductExistAsync(int id, CancellationToken cancellationToken)
    {
        
        await using (var connection = new SqlConnection(_connectionString)){
            
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Product] WHERE IdProduct = @id";
            
            await using (var command = new SqlCommand(query, connection)){
                command.Parameters.AddWithValue("@id", id);
                
                var result = await command.ExecuteScalarAsync(cancellationToken);
                
                return Convert.ToInt32(result) > 0;
            }
        }
        
    }

    public async Task<bool> DoesWareHouseExistAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString)){
            
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Warehouse] WHERE IdWarehouse = @id";
            
            await using (var command = new SqlCommand(query, connection)){
                command.Parameters.AddWithValue("@id", id);
                
                var result = await command.ExecuteScalarAsync(cancellationToken);
                
                return Convert.ToInt32(result) > 0;
            }
        }
    }

    public async Task<int> GetOrderIdAsync(int id,int amount,DateTime date, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT 
                        [dbo].[Order].[IdOrder],
                        [dbo].[Order].[IdProduct],
                        [dbo].[Order].[Amount],
                        [dbo].[Order].[CreatedAt],
                        [dbo].[Order].[FulfilledAt]
                        FROM [dbo].[Order]       
                        WHERE [dbo].[Order].[IdProduct] = @id";

            await using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if(! reader.HasRows)
                        throw new NotFoundException("Order not found");

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var _amount = reader.GetInt32(reader.GetOrdinal("Amount"));
                        if(_amount != amount)
                            throw new BadRequestException("order with this id does not match amount");
                        
                        var _createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                        if(date<_createdAt)
                            throw new ConflictException("current date is older than product date");
                        
                        var _fulfilledAt = reader.IsDBNull(reader.GetOrdinal("FulfilledAt"));
                        if(!_fulfilledAt)
                            throw new ConflictException("order is fulfilled");
                        
                        var _orderId = reader.GetInt32(reader.GetOrdinal("IdOrder"));
                        return _orderId;
                    }
                    
                }
                
            }
            
        }
        return -1;
        
    }

    public async Task<bool> DoesOrderExistAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString)){
            
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT COUNT(1) FROM [dbo].[Product_Warehouse] WHERE IdOrder = @id";
            
            await using (var command = new SqlCommand(query, connection)){
                command.Parameters.AddWithValue("@id", id);
                
                var result = await command.ExecuteScalarAsync(cancellationToken);
                
                return Convert.ToInt32(result) > 0;
            }
        }
    }
    
    public async Task<float> GetPriceAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new SqlConnection(_connectionString)){
            
            await connection.OpenAsync(cancellationToken);

            var query = @"SELECT [dbo].[Product].[Price] FROM [dbo].[Product] WHERE IdProduct = @id";
            
            await using (var command = new SqlCommand(query, connection)){
                command.Parameters.AddWithValue("@id", id);
                
                var result = await command.ExecuteScalarAsync(cancellationToken);

                return Convert.ToSingle(result);
            }
        }
    }
}