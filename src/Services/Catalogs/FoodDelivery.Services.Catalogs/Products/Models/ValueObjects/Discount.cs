using BuildingBlocks.Core.Extensions;

namespace FoodDelivery.Services.Catalogs.Products.Models.ValueObjects;

// Represents a percentage-based discount (0–100 %).
public record Discount
{
    // For EF materialization - No validation
    private Discount(decimal percentage)
    {
        Percentage = percentage;
    }

    public decimal Percentage { get; private set; }

    public static Discount Of(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Discount percentage must be between 0 and 100.");

        return new Discount(percentage);
    }

    public static Discount None => new(0);

    public decimal Apply(decimal originalPrice) => originalPrice * (1 - Percentage / 100m);

    public static implicit operator decimal(Discount discount) => discount.Percentage;

    public void Deconstruct(out decimal percentage) => percentage = Percentage;
}
