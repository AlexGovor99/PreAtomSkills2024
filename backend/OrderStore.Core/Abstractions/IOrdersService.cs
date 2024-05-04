using OrderStore.Core.Models;

namespace OrderStore.Core.Abstractions;

public interface IOrdersService
{
    Task<List<Order>> GetAll();
    Task<List<Order>> GetAllByUser(string userId);
    Task<Order?> Get(Guid id);
    Task<Guid> CreateOrder(Order order);
    Task<Guid> UpdateOrder(Order order);
    Task<Guid> DeleteOrder(Guid id);
}