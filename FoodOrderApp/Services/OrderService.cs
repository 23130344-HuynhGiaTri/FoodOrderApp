using FoodOrderApp.Models;

namespace FoodOrderApp.Services;

public class OrderService
{
    private readonly List<Order> _orders = new();

    public Order CreateOrder(string buyerId)
    {
        var order = new Order
        {
            BuyerId = buyerId,
            ExpiresAt = DateTime.UtcNow.AddSeconds(20)
        };

        _orders.Add(order);
        return order;
    }

    public void AcceptOrder(Guid orderId)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId);
        if (order != null)
        {
            order.Status = OrderStatus.Processing;
            order.ExpiresAt = DateTime.UtcNow.AddMinutes(2);
        }
    }

    public void UpdateOrderStatus(Guid orderId, OrderStatus status)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId);
        if (order != null)
        {
            order.Status = status;
        }
    }

    public List<Order> GetAllOrders()
    {
        _orders.RemoveAll(o => o.ExpiresAt <= DateTime.UtcNow);
        return _orders.ToList();
    }
}