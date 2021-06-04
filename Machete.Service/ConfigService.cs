using AutoMapper;
using AutoMapper.QueryableExtensions;
using Machete.Service;
using Machete.Service.Infrastructure;
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
        public ConfigService(IDatabaseFactory db, IMapper map) : base(db, map) {}

        public string getConfig(string key)
        {
            try
            {
                return dbset.SingleOrDefault(c => c.key == key).value;
            }
            catch(Exception e)
            {
                var msg = System.String.Format("Didn't find an entry for: {0}; Exception {1}", key, e.Message);
                throw new MacheteNullObjectException(msg);
            }
        }
    }
}
