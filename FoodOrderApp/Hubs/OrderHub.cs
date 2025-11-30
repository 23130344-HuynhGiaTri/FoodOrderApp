using Microsoft.AspNetCore.SignalR;
using FoodOrderApp.Models;
using FoodOrderApp.Services;

namespace FoodOrderApp.Hubs;

public class OrderHub : Hub
{
    private readonly OrderService _orderService;

    public OrderHub(OrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task CreateOrder(string buyerId)
    {
        var order = _orderService.CreateOrder(buyerId);
        await Clients.All.SendAsync("OrderCreated", order);
    }

    public async Task AcceptOrder(Guid orderId)
    {
        _orderService.AcceptOrder(orderId);
        var orders = _orderService.GetAllOrders();
        await Clients.All.SendAsync("OrdersUpdated", orders);
    }

    public async Task UpdateStatus(Guid orderId, int status)
    {
        _orderService.UpdateOrderStatus(orderId, (OrderStatus)status);
        var orders = _orderService.GetAllOrders();
        await Clients.All.SendAsync("OrdersUpdated", orders);
    }

    public async Task GetOrders()
    {
        var orders = _orderService.GetAllOrders();
        await Clients.Caller.SendAsync("OrdersUpdated", orders);
    }
}