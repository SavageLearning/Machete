using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Machete.Domain;
using Machete.Data.Infrastructure;
namespace Machete.Data
{
    public class RaceRepository : RepositoryBase<Race>, IRaceRepository
    {
        public RaceRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory) {}
    }
    public interface IRaceRepository : IRepository<Race> { }
    //
    public interface ILangRepository : IRepository<Language> {}

    public class LangRepository : RepositoryBase<Language>, ILangRepository
    {
        public LangRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory) { }
    }
    //
    public interface IHoodRepository : IRepository<Neighborhood> { }

    public class HoodRepository : RepositoryBase<Neighborhood>, IHoodRepository
    {
        public HoodRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory) { }
    }
    //
    public interface IIncomeRepository : IRepository<Income> { }

    public class IncomeRepository : RepositoryBase<Income>, IIncomeRepository
    {
        public IncomeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory) { }
    }
    

}
