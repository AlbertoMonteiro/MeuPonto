using System.Data.Linq;
using Meu_Ponto.Models;

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

        public Table<Batida> Batidas
        {
            get { return GetTable<Batida>(); }
        }
    }

}
