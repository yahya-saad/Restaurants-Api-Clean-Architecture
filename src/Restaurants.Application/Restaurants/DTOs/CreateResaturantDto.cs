using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.DTOs;
public class CreateResaturantDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }

    public string? ContractEmail { get; set; }
    public string? ContractNumber { get; set; }

    public Address? Address { get; set; }
}
