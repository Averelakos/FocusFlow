using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TaskConfiguration : BaseEntityConfiguration<Task>
{
    public TaskConfiguration() : base()
    {
    }

    public override void Configure(EntityTypeBuilder<Task> entity)
    {
        base.Configure(entity);
        ConfigureTask(entity);
    }

    public static void ConfigureTask(EntityTypeBuilder<Task> entity)
    {   
        entity.ToTable("Tasks");
        entity.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(u => u.Description)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(x => x.DueDate)
            .HasColumnType("datetime2(7)")
            .HasDefaultValueSql("GETUTCDATE()");
        
        entity.Property(x => x.CompletedAt)
            .HasColumnType("datetime2(7)")
            .HasDefaultValueSql("GETUTCDATE()");

        entity.HasOne(p => p.Project)
            .WithMany(u => u.Tasks)
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(p => p.AssignedTo)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(p => p.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}