using System.Data.Linq;
using MeuPonto.Common.Models;

namespace MeuPonto.Common.Repositorios
{
    public class CacheContext : DataContext
    {
        public CacheContext(string connectionString = "Data Source=isostore:/CacheDB.sdf")
            : base(connectionString)
        {
            //if (ChangeConflicts.Any())
                //DeleteDatabase();
            
            if (!DatabaseExists())
                CreateDatabase();
            
            DeferredLoadingEnabled = true;
            /*var dbUpdater = this.CreateDatabaseSchemaUpdater();
            dbUpdater.DatabaseSchemaVersion = 2;
            dbUpdater.Execute();*/

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
