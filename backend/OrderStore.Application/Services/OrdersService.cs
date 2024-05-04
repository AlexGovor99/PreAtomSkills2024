

using OrderStore.Core.Abstractions;
using OrderStore.Core.Models;

namespace OrderStore.Application.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _repository;
    
    public OrdersService(IOrdersRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Order>> GetAll()
    {
        return await _repository.Get();
    }

    public async Task<List<Order>> GetAllByUser(string userId)
    {
        return (await _repository.Get())
            .Where(x => x.UserId == userId)
            .ToList();
    }

    public async Task<Order?> Get(Guid id)
    {
        var orders = await _repository.Get();
        return orders.FirstOrDefault(x => x.Id == id);
    }

    public async Task<Guid> CreateOrder(Order order)
    {
        return await _repository.Create(order);
    }

    public async Task<Guid> UpdateOrder(Order order)
    {
        return await _repository.Update(order);
    }

    public async Task<Guid> DeleteOrder(Guid id)
    {
        return await _repository.Delete(id);
    }
}