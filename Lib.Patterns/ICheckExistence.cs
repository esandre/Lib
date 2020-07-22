using System;
using System.Threading.Tasks;

namespace Lib.Patterns
{
    public interface IBuildsExceptionIfNotExistsAsync<in T>
    {
        Exception BuildExceptionIfNotExists(T element);
    }

    public interface ICheckExistence<T> : IBuildsExceptionIfNotExistsAsync<T>
    {
        bool Exists(T element);

        Existe<T> Certify(T element);
    }

    public interface ICheckExistenceAsync<T> : IBuildsExceptionIfNotExistsAsync<T>
    {
        Task<bool> ExistsAsync(T element);

        ExisteAsync<T> Certify(T element);
    }
}
