using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasData
        (
        new Student
        {
            Id = -1,
            Name = "IT_Solutions Ltd",
            Address = "583 Wall Dr. Gwynn Oak, MD 21207",
            Country = "USA"
        },
        new Student
        {
            Id = -2,
            Name = "Admin_Solutions Ltd",
            Address = "312 Forest Avenue, BF 923",
            Country = "USA"
        }
        );
    }
}