using System.ComponentModel.DataAnnotations;

namespace APBD_Kolokwium2.DTOs;

public class CreateDeliveryRequestDto
{
    [Range(1, int.MaxValue)]
    public int CustomerId { get; set; }

    [Required]
    [MaxLength(17)]
    public string LicenceNumber { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public List<CreateDeliveryProductDto> Products { get; set; } = new();

}