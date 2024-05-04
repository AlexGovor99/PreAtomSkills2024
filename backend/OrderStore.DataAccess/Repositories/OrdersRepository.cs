using Microsoft.EntityFrameworkCore;
using OrderStore.Core.Abstractions;
using OrderStore.Core.Models;
using OrderStore.DataAccess.Entities;

namespace OrderStore.DataAccess.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrderStoreDbContext _dbContext;
    
    public OrdersRepository(OrderStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> Get()
    {
        var bookEntities = await _dbContext.Orders
            .AsNoTracking()
            .ToListAsync();

        var books = bookEntities
            .Select(x => Order.Create(x.Id, x.Name, x.UserId, x.Status, x.EditDate, x.Comment, x.FileId).Order)
            .ToList();
        
        return books!;
    }

    public async Task<Guid> Create(Order order)
    {
        var orderEntity = new OrderEntity()
        {
            Id = order.Id,
            Name = order.Name,
            UserId = order.UserId,
            FileId = order.FileId,
            Comment = order.Comment,
            EditDate = order.EditDate,
            Status = order.Status,
        };

        await _dbContext.Orders.AddAsync(orderEntity);
        await _dbContext.SaveChangesAsync();

        return orderEntity.Id;
    }

    public async Task<Guid> Update(Order order)
    {
        await _dbContext.Orders
            .Where(x => x.Id == order.Id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(b => b.Name, b => order.Name)
                .SetProperty(b => b.UserId, b => order.UserId)
                .SetProperty(b => b.Status, b => order.Status)
                .SetProperty(b => b.EditDate, b => order.EditDate)
                .SetProperty(b => b.Comment, b => order.Comment)
                .SetProperty(b => b.FileId, b => order.FileId));

        return order.Id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _dbContext.Orders
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}