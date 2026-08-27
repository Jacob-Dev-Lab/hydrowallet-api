using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities;
using Wallet.Domain.ValueObjects;

namespace Wallet.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.OtherNames)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.DateOfBirth)
                .HasConversion(
                    dob => dob.Value,
                    dob => DateOfBirth.Create(
                        dob.ToString("yyyy-MM-dd")
                    ))
                .IsRequired();

            builder.Property(u => u.Email)
                .HasConversion(
                    e => e.Value, 
                    e => Email.Create(e))
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.MobileNumber)
                .HasConversion(
                    mn => mn.Value, 
                    mn => MobileNumber.Create(mn))
                .IsRequired()
                .HasMaxLength(15);

            builder.HasIndex(u => u.MobileNumber)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.UserStatus)
                .IsRequired();

            builder.Property(u => u.RegisteredAt)
                .IsRequired();
        }
    }
}
