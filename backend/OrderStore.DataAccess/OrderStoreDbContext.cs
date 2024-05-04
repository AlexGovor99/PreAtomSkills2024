using Microsoft.EntityFrameworkCore;
using OrderStore.DataAccess.Entities;

namespace OrderStore.DataAccess;

public class OrderStoreDbContext : DbContext
{
    public OrderStoreDbContext(DbContextOptions<OrderStoreDbContext> options) : base(options)
    {
    }
    
    public DbSet<OrderEntity> Orders { get; set; }
}