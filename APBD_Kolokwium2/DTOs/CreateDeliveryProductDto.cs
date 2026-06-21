using System.ComponentModel.DataAnnotations;

namespace APBD_Kolokwium2.DTOs;

public class CreateDeliveryProductDto
{
    [Required] 
    [MaxLength(100)] 
    public string Name { get; set; } = null!;
    
    [Range(1, int.MaxValue)]
    public int Amount { get; set; }
    
}