using System.ComponentModel.DataAnnotations;

namespace APBD_Kolokwium2.Entities;

public class Driver
{
    public int Driverid { get; set; }

    [MaxLength(100)] 
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    
    [MaxLength(17)]
    public string LicenceNumber { get; set; } = null!;
    
    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

}