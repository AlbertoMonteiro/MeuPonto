using System.Linq;
using Meu_Ponto.Models;
using System.Data.Linq;

namespace Meu_Ponto.Data
{
    public class CacheContext : DataContext
    {
        public CacheContext(string connectionString = "Data Source=isostore:/CacheDB.sdf")
            : base(connectionString)
        {
            //if (this.ChangeConflicts.Any())
            {
                //DeleteDatabase();
            }
            if (!DatabaseExists())
            {
                
                CreateDatabase();
            }
            DeferredLoadingEnabled = true;
        }

        public Table<Configuracao> Configuracoes
        {
            get { return GetTable<Configuracao>(); }
        }
    }

}
