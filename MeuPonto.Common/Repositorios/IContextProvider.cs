namespace MeuPonto.Common.Repositorios
{
    public interface IContextProvider
    {
        CacheContext CacheContext { get; }
    }
}