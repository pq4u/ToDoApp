using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApp.Common.Models;

namespace ToDoApp.DB.Configurations;

public class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.PercentCompleted)
            .IsRequired(false);

        builder.Property(x => x.IsCompleted)
            .HasDefaultValue(true)
            .ValueGeneratedNever();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.ExpiryAt)
            .IsRequired();
    }
}