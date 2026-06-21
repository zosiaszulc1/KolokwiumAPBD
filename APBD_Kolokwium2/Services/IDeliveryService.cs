using APBD_Kolokwium2.DTOs;

namespace APBD_Kolokwium2.Services;

public interface IDeliveryService
{
    Task<GetDeliveryResponseDto?> GetDeliveryAsync(int id);
    Task<DeliveryOperationResult> CreateDeliveryAsync(CreateDeliveryRequestDto request);
    
}