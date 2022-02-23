using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository<User>
{
    public UserRepository(RepositoryContext repositoryContext)
    : base(repositoryContext)
    {
    }
}