namespace APBD_Kolokwium2.DTOs;

public class GetDeliveryResponseDto
{
    public DateTime Date { get; set; }
    public CustomerDto Customer { get; set; } = null!;
    public DriverDto Driver { get; set; } = null!;
    public List<ProductDto> Product { get; set; } = new();
}