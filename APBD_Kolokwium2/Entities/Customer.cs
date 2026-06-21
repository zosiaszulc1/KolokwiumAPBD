using System.ComponentModel.DataAnnotations;

namespace APBD_Kolokwium2.Entities;

public class Customer
{
    public int Customerid { get; set; }
    
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}