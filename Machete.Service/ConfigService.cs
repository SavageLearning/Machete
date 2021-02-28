using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Machete.Service
{
    public interface IConfigService : IService<Config>
    {
        //dataTableResult<DTO.ConfigList> GetIndexView(viewOptions o);
        string getConfig(string key);
    }

    public class ConfigService : ServiceBase2<Config>, IConfigService
    {
        private List<Config> config { get; set; }

        public ConfigService(IDatabaseFactory db, IMapper map) : base(db, map) {}

        public string getConfig(string key)
        {
            if (config == null)
            {
                config = GetAll().ToList();
            }
            try
            {
                return config.Where(c => c.key == key).SingleOrDefault().value;
            }
            catch(Exception e)
            {
                var msg = System.String.Format("Didn't find an entry for: {0}; Exception {1}", key, e.Message);
                throw new MacheteNullObjectException(msg);
            }
        }
    }
}
