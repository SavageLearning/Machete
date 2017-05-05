using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Data
{
    public interface IConfigRepository : IRepository<Config>
    {
    }
    public class ConfigRepository : RepositoryBase<Config>, IConfigRepository
    {

        public ConfigRepository(IDatabaseFactory dbFactory) : base(dbFactory)
        { }

    }
}