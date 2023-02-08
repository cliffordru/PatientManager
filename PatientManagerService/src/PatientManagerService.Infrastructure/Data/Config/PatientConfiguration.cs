using PatientManagerService.Core.PatientAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PatientManagerService.Infrastructure.Data.Config;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
  public void Configure(EntityTypeBuilder<Patient> builder)
  {
    builder.Property(p => p.FirstName)
        .HasMaxLength(35)
        .IsRequired();

    builder.Property(p => p.LastName)
        .HasMaxLength(35)
        .IsRequired();

    builder.Property(p => p.Birthday)
        .HasMaxLength(30)
        .IsRequired();

    builder.Property(p => p.Gender)
        .HasMaxLength(10)
        .IsRequired();
  }
}
