using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using Machete.Data.Infrastructure;

namespace Machete.Service
{
    public interface IRaceService { IEnumerable<Race> Lookup(); }
    public interface ILangService { IEnumerable<Language> Lookup(); }
    public interface IHoodService { IEnumerable<Neighborhood> Lookup(); }
    public interface IIncomeService { IEnumerable<Income> Lookup(); }

    public class RaceService : IRaceService
    {
        private readonly IRaceRepository raceRepository;

        public RaceService(IRaceRepository raceRepository)
        {
            this.raceRepository = raceRepository;
        }

        public IEnumerable<Race> Lookup()
        {
            var races =  raceRepository.GetAll();
            return races;
        }
    }

    public class LangService : ILangService
    {
        private readonly ILangRepository langRepository;

        public LangService(ILangRepository langRepository)
        {
            this.langRepository = langRepository;
        }

        public IEnumerable<Language> Lookup()
        {
            var langs = langRepository.GetAll();
            return langs;
        }
    }

    public class HoodService : IHoodService
    {
        private readonly IHoodRepository hoodRepository;

        public HoodService(IHoodRepository hoodRepository)
        {
            this.hoodRepository = hoodRepository;
        }

        public IEnumerable<Neighborhood> Lookup()
        {
            var hoods = hoodRepository.GetAll();
            return hoods;
        }
    }

    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository incomeRepository;

        public IncomeService(IIncomeRepository incomeRepository)
        {
            this.incomeRepository = incomeRepository;
        }

        public IEnumerable<Income> Lookup()
        {
            var incomes = incomeRepository.GetAll();
            return incomes;
        }
    }
}
