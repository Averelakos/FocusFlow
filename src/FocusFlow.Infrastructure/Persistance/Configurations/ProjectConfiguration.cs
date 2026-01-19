using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProjectConfiguration : BaseEntityConfiguration<Project>
{
    public ProjectConfiguration() : base()
    {
    }

    public override void Configure(EntityTypeBuilder<Project> entity)
    {
        base.Configure(entity);
        ConfigureProject(entity);
    }

    public static void ConfigureProject(EntityTypeBuilder<Project> entity)
    {   
        entity.ToTable("Projects");
        entity.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(u => u.Description)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(x => x.StartDate)
            .HasColumnType("datetime2(7)")
            .HasDefaultValueSql("GETUTCDATE()");
        
        entity.Property(x => x.EndDate)
            .HasColumnType("datetime2(7)")
            .HasDefaultValueSql("GETUTCDATE()");

        entity.HasOne(p => p.Owner)
            .WithMany(u => u.OwnedProjects)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}