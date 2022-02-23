using Entities;

namespace Contracts;

public interface IRepositoryManager
{
    IStudentRepository<Student> Student { get; }
    IEmployeeRepository Employee { get; }

    void Save();
}