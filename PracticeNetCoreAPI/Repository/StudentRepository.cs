using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public class StudentRepository : RepositoryBase<Student>, IStudentRepository<Student>
{
    public StudentRepository(RepositoryContext repositoryContext)
    : base(repositoryContext)
    {
    }
}