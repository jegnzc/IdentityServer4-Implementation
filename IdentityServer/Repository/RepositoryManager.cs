using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private RepositoryContext _repositoryContext;
    private IUserRepository<User> _studentRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public IUserRepository<User> AspNetUser
    {
        get
        {
            if (_studentRepository == null)
                _studentRepository = new UserRepository(_repositoryContext);
            return _studentRepository;
        }
    }

    public void Save() => _repositoryContext.SaveChanges();
}