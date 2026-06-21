namespace APBD_Kolokwium2.Entities;

public class ProductDelivery
{
    public int ProductId { get; set; }
    public int DeliveryId { get; set; }
    public int Amount { get; set; }
    public Product Product { get; set; } = null!;
    public Delivery Delivery { get; set; } = null!;
}