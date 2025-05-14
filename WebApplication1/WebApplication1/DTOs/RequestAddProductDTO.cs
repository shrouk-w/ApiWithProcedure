﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class RequestAddProductDTO
{
    [Required]
    public int IdProduct{ get; set; }
    [Required]
    public int IdWarehouse{ get; set; }
    [Required]
    public int Amount{ get; set; }
}