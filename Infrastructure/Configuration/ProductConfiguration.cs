using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public async  void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Name)
            .IsRequired();
    }
}
