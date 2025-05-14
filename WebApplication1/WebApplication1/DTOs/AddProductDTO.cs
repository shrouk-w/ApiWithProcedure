using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class AddProductDTO
{
    [Required]
    int IdProduct{ get; set; }
    [Required]
    int IdWarehouse{ get; set; }
    [Required]
    int Amount{ get; set; }
}