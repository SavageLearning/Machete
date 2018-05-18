using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface IConfigService : IService<Config>
    {
        //dataTableResult<DTO.ConfigList> GetIndexView(viewOptions o);
        string getConfig(string key);
    }

    public class ConfigService : ServiceBase<Config>, IConfigService
    {
        private readonly IMapper map;
        private List<Config> config { get; set; }

        public ConfigService(IConfigRepository repo,
                               IUnitOfWork unitOfWork,
                               IMapper map)
                : base(repo, unitOfWork)
        {
            this.map = map;
            this.logPrefix = "Config";
        }

        public string getConfig(string key)
        {
            if (config == null)
            {
                config = GetAll().ToList();
            }
            return config.Where(c => c.key == key).SingleOrDefault().value;
        }
    }
}
