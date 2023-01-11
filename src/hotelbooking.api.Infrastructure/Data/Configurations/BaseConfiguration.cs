using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using hotelbooking.api.SharedKernel;

namespace hotelbooking.api.Infrastructure.Data.Configurations;

public abstract class BaseEntityConfiguration<TBaseEntity> : IEntityTypeConfiguration<TBaseEntity>
    where TBaseEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TBaseEntity> builder)
    {
        builder.Ignore(e => e.Events);

        builder.HasIndex(e => e.CreatedDt);
        builder.HasIndex(e => e.LastModifiedDt);

        builder.HasQueryFilter(e => e.IsActive && e.DeletedDt == null);

        EntityConfiguration(builder);
    }

    protected abstract void EntityConfiguration(EntityTypeBuilder<TBaseEntity> builder);
}