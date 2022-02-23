using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasData
        (
            new Employee
            {
                Id = -1,
                Name = "Sam Raiden",
                Age = 26,
                Position = "Software developer",
            },
            new Employee
            {
                Id = -2,
                Name = "Jana McLeaf",
                Age = 30,
                Position = "Software developer",
            },
            new Employee
            {
                Id = -3,
                Name = "Kane Miller",
                Age = 35,
                Position = "Administrator",
            }
        );
    }
}