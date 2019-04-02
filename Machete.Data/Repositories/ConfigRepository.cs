using Machete.Data.Infrastructure;
using Machete.Domain;

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