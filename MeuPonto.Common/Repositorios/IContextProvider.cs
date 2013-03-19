using System;

namespace MeuPonto.Common.Repositorios
{
    public interface IContextProvider : IDisposable
    {
        CacheContext CacheContext { get; }
    }
}