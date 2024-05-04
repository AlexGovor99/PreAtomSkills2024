using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderStore.DataAccess.Entities;

namespace OrderStore.DataAccess.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.FileId)
            .IsRequired();
        
        builder.Property(x => x.Comment)
            .IsRequired();
        
        builder.Property(x => x.EditDate)
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .IsRequired();
    }
}