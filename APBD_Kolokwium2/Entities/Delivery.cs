namespace APBD_Kolokwium2.Entities;

public class Delivery
{
    public int DeliveryId { get; set; }
    public int CustomerId { get; set; }
    public int DriverId { get; set; }
    public DateTime Date { get; set; }
    public Customer Customer { get; set; } = null!;
    public Driver Driver { get; set; } = null!;
    
    public ICollection<ProductDelivery> ProductDeliveries { get; set; } = new List<ProductDelivery>();
}