using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using System.Data.Entity;

namespace Machete.Data
{
    public static class MacheteLookup    {
        //public static List<Race> races { get; private set; }
        //public static List<Language> languages { get; private set; }
        //public static List<Neighborhood> neighborhoods { get; private set; }
        //public static List<Income> incomes { get; private set; }
        //public static List<Skill> skills { get; private set; }
        //public static List<TypeOfWork> typesofwork { get; private set; }

        public static void Initialize(MacheteContext context) {
            init_races(context);
            init_neighborhoods(context);
            init_incomes(context);
            init_languages(context);
            init_skills(context);
            init_typesofwork(context);
        }
        private static void init_races(MacheteContext context) {
            new List<Race>
            {
                //TODO: localize: How do I localize thses lookup strings?       
                new Race { racelabel = "Afroamerican" },
                new Race { racelabel = "Asian"},
                new Race { racelabel = "Caucausian" },
                new Race { racelabel = "Hawaiian" },
                new Race { racelabel = "Latino" },
                new Race { racelabel = "Native American"},
                new Race { racelabel = "Other"}
            }.ForEach(u => context.Races.Add(u));
        }

        private static void init_languages(MacheteContext context)
        {
            new List<Language>
            {
                new Language { language = "English", level="conversational"},
                new Language { language = "English", level="limited"},
                new Language { language = "English", level="none"}
            }.ForEach(u => context.Languages.Add(u));
        }

        private static void init_neighborhoods(MacheteContext context)
        {
            new List<Neighborhood>
            {
                new Neighborhood { neighborhood = "Seattle"},
                new Neighborhood { neighborhood = "Capitol Hill"},
                new Neighborhood { neighborhood = "Central District"},
                new Neighborhood { neighborhood = "South Park"},
                new Neighborhood { neighborhood = "Kent"}, 
                new Neighborhood { neighborhood = "Auburn"}
            }.ForEach(u => context.Neighborhoods.Add(u));
        }

        private static void init_incomes(MacheteContext context)
        {
            new List<Income>
            {
                new Income { incomelabel = "Less than $10,000"},
                new Income { incomelabel = "Between $10,000 and $25,000"},
                new Income { incomelabel = "Above $25,000"}
            }.ForEach(u => context.Incomes.Add(u));
        }

        private static void init_skills(MacheteContext context)
        {
            new List<Skill>
            {
                new Skill {skill = "painting", level = "apprentice"},
                new Skill {skill = "painting", level = "competent"},
                new Skill {skill = "painting", level = "master"},
                new Skill {skill = "construction", level = "apprentice"},
                new Skill {skill = "construction", level = "competent"},
                new Skill {skill = "construction", level = "master"}
            }.ForEach(u => context.Skills.Add(u));
        }

        private static void init_typesofwork(MacheteContext context)
        {
            new List<TypeOfWork>
            {
                new TypeOfWork { worklabel = "Construction"},
                new TypeOfWork { worklabel = "Janitorial"},
                new TypeOfWork { worklabel = "Landscaping"},
                new TypeOfWork { worklabel = "Restaurant"},
                new TypeOfWork { worklabel = "Service"},
                new TypeOfWork { worklabel = "Factory"}
            }.ForEach(u => context.TypesOfWork.Add(u));
        }
    }
    
}
