using System;
using Lib.Patterns;

namespace Lib.Mocked.StubFactory
{
    /// <inheritdoc />
    /// <summary>
    /// Stub responsible factory
    /// </summary>
    public interface IStubFactory : IResponsibleFactory<Type, object>
    {
    }
}
