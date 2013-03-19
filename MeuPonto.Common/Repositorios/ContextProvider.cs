namespace MeuPonto.Common.Repositorios
{
    public class ContextProvider : IContextProvider
    {
        private CacheContext _cacheContext;

        public ContextProvider()
        {
            _cacheContext = _cacheContext ?? new CacheContext();
        }

        public CacheContext CacheContext { get { return _cacheContext; } }

        public void Dispose()
        {
            _cacheContext.SubmitChanges();
            _cacheContext.Dispose();
        }
    }
}