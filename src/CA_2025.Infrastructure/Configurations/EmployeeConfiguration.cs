using CA_2025.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CA_2025.Infrastructure.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.OwnsOne(x => x.PersonelInformation, builder =>
        {
            builder.Property(x => x.TCNo).HasColumnName("TCNO");
            builder.Property(x => x.Phone1).HasColumnName("Phone1");
            builder.Property(x => x.Phone1).HasColumnName("Phone2");
            builder.Property(x => x.Email).HasColumnName("Email");
        });

        builder.OwnsOne(x => x.Address, builder =>
        {
            builder.Property(x => x.Country).HasColumnName("Country");
            builder.Property(x => x.Town).HasColumnName("Town");
            builder.Property(x => x.City).HasColumnName("City");
            builder.Property(x => x.FullAddress).HasColumnName("FullAddress");
        });

        builder.Property(x => x.Salary).HasColumnName("money");
    }
}
