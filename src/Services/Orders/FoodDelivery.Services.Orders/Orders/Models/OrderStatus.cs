namespace FoodDelivery.Services.Orders.Orders.Models;

public enum OrderStatus
{
    Pending = 1,
    Confirmed,
    Preparing,
    OutForDelivery,
    Delivered,
    Cancelled,
}
