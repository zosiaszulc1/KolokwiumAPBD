using APBD_Kolokwium2.Data;
using APBD_Kolokwium2.DTOs;
using APBD_Kolokwium2.Entities;
using Microsoft.EntityFrameworkCore;

namespace APBD_Kolokwium2.Services;

public class DeliveryService : IDeliveryService
{
    private readonly AppDbContext _context;

    public DeliveryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetDeliveryResponseDto?> GetDeliveryAsync(int id)
    {
        return await _context.Deliveries
           .Where(d => d.DeliveryId == id)
            .Select(d => new GetDeliveryResponseDto
            {
                Date = d.Date,

                Customer = new CustomerDto
                {
                    FirstName = d.Customer.FirstName,
                    LastName = d.Customer.LastName,
                    DateOfBirth = d.Customer.DateOfBirth
                },

                Driver = new DriverDto
                {
                    FirstName = d.Driver.FirstName,
                    LastName = d.Driver.LastName,
                    LicenceNumber = d.Driver.LicenceNumber
                },

                Products = d.ProductDeliveries
                    .Select(pd => new ProductDto
                    {
                        Name = pd.Product.Name,
                        Price = pd.Product.Price,
                        Amount = pd.Amount
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DeliveryOperationResult> CreateDeliveryAsync(CreateDeliveryRequestDto request)
    {
        if (request.Products == null || request.Products.Count == 0)
        {
            return new DeliveryOperationResult
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Products list cannot be empty."
            };
        }

        if (request.Products.Any(p => string.IsNullOrWhiteSpace(p.Name) || p.Amount <= 0))
        {
            return new DeliveryOperationResult
            {
                Success = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Product name cannot be empty and amount must be greater than 0."
            };
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var customerExists = await _context.Customers
                .AnyAsync(c => c.CustomerId == request.CustomerId);

            if (!customerExists)
            {
                return new DeliveryOperationResult
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Customer with id {request.CustomerId} was not found."
                };
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.LicenceNumber == request.LicenceNumber);

            if (driver == null)
            {
                return new DeliveryOperationResult
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Driver with licence number {request.LicenceNumber} was not found."
                };
            }

            var groupedProducts = request.Products
                .GroupBy(p => p.Name)
                .Select(g => new CreateDeliveryProductDto
                {
                    Name = g.Key,
                    Amount = g.Sum(p => p.Amount)
                })
                .ToList();

            var productNames = groupedProducts
                .Select(p => p.Name)
                .ToList();

            var productsFromDatabase = await _context.Products
                .Where(p => productNames.Contains(p.Name))
                .ToListAsync();

            if (productsFromDatabase.Count != productNames.Count)
            {
                var existingNames = productsFromDatabase
                    .Select(p => p.Name)
                    .ToList();

                var missingNames = productNames
                    .Where(name => !existingNames.Contains(name))
                    .ToList();

                return new DeliveryOperationResult
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Products not found: {string.Join(", ", missingNames)}."
                };
            }

            var delivery = new Delivery
            {
                CustomerId = request.CustomerId,
                DriverId = driver.DriverId,
                Date = DateTime.Now
            };

            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();

            foreach (var requestProduct in groupedProducts)
            {
                var product = productsFromDatabase
                    .First(p => p.Name == requestProduct.Name);

                var productDelivery = new ProductDelivery
                {
                    DeliveryId = delivery.DeliveryId,
                    ProductId = product.ProductId,
                    Amount = requestProduct.Amount
                };

                await _context.ProductDeliveries.AddAsync(productDelivery);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new DeliveryOperationResult
            {
                Success = true,
                StatusCode = StatusCodes.Status201Created,
                Message = "Delivery was created successfully.",
                DeliveryId = delivery.DeliveryId
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

