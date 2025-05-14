using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class AddProductToDB_DTO
{
    public int IdOrder{ get; set; }
    
    public float Price{ get; set; }
    public int IdProduct{ get; set; }
    
    public int IdWarehouse{ get; set; }
    
    public int Amount{ get; set; }
    
    public DateTime Date{ get; set; }
}