using OrderStore.Core.Models;

namespace OrderStore.Core.Abstractions;

public interface IOrdersRepository
{
    Task<List<Order>> Get();
    Task<Guid> Create(Order order);
    Task<Guid> Update(Order order);
    Task<Guid> Delete(Guid id);
}