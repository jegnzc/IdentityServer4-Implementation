using Entities;

namespace Contracts;

public interface IUserRepository<T> : IRepositoryBase<T> where T : class
{
}