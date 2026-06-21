using System.ComponentModel.DataAnnotations;

namespace APBD_Kolokwium2.Entities;

public class Product
{
    public int Productid { get; set; }

    [MaxLength(100)] 
    public string Name { get; set; } = null!;
    
    public decimal Price { get; set; }
    
    public ICollection<ProductDelivery> ProductDeliveries { get; set; } = new List<ProductDelivery>();
    
}