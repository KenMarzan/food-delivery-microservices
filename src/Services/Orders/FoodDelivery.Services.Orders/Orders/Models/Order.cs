using BuildingBlocks.Core.Domain;
using FoodDelivery.Services.Orders.Orders.ValueObjects;

namespace FoodDelivery.Services.Orders.Orders.Models;

// https://learn.microsoft.com/en-us/ef/core/modeling/constructors
// https://github.com/dotnet/efcore/issues/29940
public class Order : Aggregate<OrderId>
{
    // EF
    // this constructor is needed when we have a parameter constructor that has some navigation property classes in the parameters and ef will skip it and try to find other constructor, here default constructor (maybe will fix .net 8)
    private Order() { }

    public CustomerInfo Customer { get; private set; } = default!;
    public ProductInfo Product { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }

    public static Order Create(CustomerInfo customerInfo, ProductInfo productInfo)
    {
        return new Order
        {
            Customer = customerInfo,
            Product = productInfo,
            Status = OrderStatus.Pending,
        };
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm an order that is in '{Status}' status.");

        Status = OrderStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void StartPreparing()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException($"Cannot start preparing an order that is in '{Status}' status.");

        Status = OrderStatus.Preparing;
    }

    public void DispatchForDelivery()
    {
        if (Status != OrderStatus.Preparing)
            throw new InvalidOperationException($"Cannot dispatch an order that is in '{Status}' status.");

        Status = OrderStatus.OutForDelivery;
    }

    public void MarkDelivered()
    {
        if (Status != OrderStatus.OutForDelivery)
            throw new InvalidOperationException($"Cannot mark as delivered an order that is in '{Status}' status.");

        Status = OrderStatus.Delivered;
    }

    public void Cancel(string? reason = null)
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel an order that is already '{Status}'.");

        Status = OrderStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = reason;
    }
}
