namespace APBD_Kolokwium2.Services;

public class DeliveryOperationResult
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public int? DeliveryId { get; set; }
}