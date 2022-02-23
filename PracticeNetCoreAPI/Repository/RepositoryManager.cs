using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private RepositoryContext _repositoryContext;
    private IStudentRepository<Student> _studentRepository;
    private IEmployeeRepository _employeeRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public IStudentRepository<Student> Student
    {
        get
        {
            if (_studentRepository == null)
                _studentRepository = new StudentRepository(_repositoryContext);
            return _studentRepository;
        }
    }

    public IEmployeeRepository Employee
    {
        get
        {
            if (_employeeRepository == null)
                _employeeRepository = new EmployeeRepository(_repositoryContext);
            return _employeeRepository;
        }
    }

    public void Save() => _repositoryContext.SaveChanges();
}