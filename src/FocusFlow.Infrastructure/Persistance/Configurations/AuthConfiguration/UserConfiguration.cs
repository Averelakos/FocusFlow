using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public UserConfiguration() : base()
    {
    }

    public override void Configure(EntityTypeBuilder<User> entity)
    {
        base.Configure(entity);
        ConfigureUser(entity);
    }

    public static void ConfigureUser(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");
        entity.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(100);

        Seed(entity);
    }

    private static void Seed(EntityTypeBuilder<User> entity)
    {
        entity.HasData(
            new User
            {
                Id = -1,
                Username = "System",
                Email = "System@focusflow.com",
                FullName = "System User",
                Created = DateTime.UtcNow,
                CreatedById = null,
                LastUpdated = DateTime.UtcNow,
                LastUpdatedById = null,
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>()
            });
    }
}