using Entities;

namespace Contracts;

public interface IRepositoryManager
{
    IUserRepository<User> AspNetUser { get; }

    void Save();
}