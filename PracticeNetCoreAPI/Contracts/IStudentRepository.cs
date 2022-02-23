using Entities;

namespace Contracts;

public interface IStudentRepository<T> : IRepositoryBase<T> where T : class
{
}