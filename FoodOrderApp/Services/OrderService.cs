using FoodOrderApp.Models;

namespace FoodOrderApp.Services;

public class OrderService : IDisposable
{
    private readonly List<Order> _orders = new();
    private readonly System.Timers.Timer _timer;
    public event Action? OnOrdersChanged;

    public OrderService()
    {
        _timer = new System.Timers.Timer(10);
        _timer.Elapsed += (s, e) =>
        {
            CleanupExpiredOrders();
            NotifyOrdersChanged();
        };
        _timer.Start();
    }

    public List<Order> GetAllOrders()
    {
        return _orders.ToList();
    }

    public Order CreateOrder(string buyerId)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            BuyerId = buyerId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddSeconds(10)
        };
        _orders.Add(order);
        NotifyOrdersChanged();
        return order;
    }

    public void AcceptOrder(Guid orderId)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId);
        if (order != null)
        {
            order.Status = OrderStatus.Processing;
            order.ExpiresAt = DateTime.UtcNow.AddSeconds(20);
            NotifyOrdersChanged();
        }
    }

    public void UpdateOrderStatus(Guid orderId, OrderStatus status)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId);
        if (order != null)
        {
            order.Status = status;
            NotifyOrdersChanged();
        }
    }

    private void CleanupExpiredOrders()
    {
        _orders.RemoveAll(o => o.ExpiresAt <= DateTime.UtcNow);
    }

    private void NotifyOrdersChanged()
    {
        OnOrdersChanged?.Invoke();
    }

    public void Dispose()
    {
        _timer?.Stop();
        _timer?.Dispose();
    }
}