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

        public override Config Create(Config record, string user)
        {
            var result = base.Create(record, user);
            uow.Commit();
            return result;
        }

        public override void Save(Config record, string user)
        {
            base.Save(record, user);
            uow.Commit();
        }

        public override void Delete(int id, string user)
        {
            base.Delete(id, user);
            uow.Commit();
        }

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
