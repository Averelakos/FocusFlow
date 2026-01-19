using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> entity)
    {
        #region Common Properties
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        entity.Property(x => x.Created)
            .HasColumnType("datetime2(7)")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        entity.Property(e => e.LastUpdated)
            .HasColumnType("datetime2(7)")
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        #endregion Common Properties
        
        #region Navigation Properties
        entity.HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.LastUpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.LastUpdatedById)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion Navigation Properties
    }

}