namespace FoodOrderApp.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string BuyerId { get; set; } = string.Empty;
}

public enum OrderStatus
{
    Pending,
    Processing,
    Delivered
}