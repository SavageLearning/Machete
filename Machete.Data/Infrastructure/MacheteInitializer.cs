using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Database;
using Machete.Domain;

namespace Machete.Data
{
    public class MacheteInitializer : DropCreateDatabaseIfModelChanges<MacheteContext>
    {
        protected override void Seed(MacheteContext macheteDB)
        {
            //Initialize Lookups
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
            }.ForEach(u => macheteDB.Races.Add(u));

            new List<Language>
            {
                new Language { language = "English", level="conversational"},
                new Language { language = "English", level="limited"},
                new Language { language = "English", level="none"}
            }.ForEach(u => macheteDB.Languages.Add(u));

            new List<Neighborhood>
            {
                new Neighborhood { neighborhood = "Seattle"},
                new Neighborhood { neighborhood = "Capitol Hill"},
                new Neighborhood { neighborhood = "Central District"},
                new Neighborhood { neighborhood = "South Park"},
                new Neighborhood { neighborhood = "Kent"}, 
                new Neighborhood { neighborhood = "Auburn"}
            }.ForEach(u => macheteDB.Neighborhoods.Add(u));

            new List<Income>
            {
                new Income { incomelabel = "Less than $10,000"},
                new Income { incomelabel = "Between $10,000 and $25,000"},
                new Income { incomelabel = "Above $25,000"}
            }.ForEach(u => macheteDB.Incomes.Add(u));

            new List<Skill>
            {
                new Skill {skill = "painting", level = "apprentice"},
                new Skill {skill = "painting", level = "competent"},
                new Skill {skill = "painting", level = "master"},
                new Skill {skill = "construction", level = "apprentice"},
                new Skill {skill = "construction", level = "competent"},
                new Skill {skill = "construction", level = "master"}
            }.ForEach(u => macheteDB.Skills.Add(u));

            new List<TypeOfWork>
            {
                new TypeOfWork { worklabel = "Construction"},
                new TypeOfWork { worklabel = "Janitorial"},
                new TypeOfWork { worklabel = "Landscaping"},
                new TypeOfWork { worklabel = "Restaurant"},
                new TypeOfWork { worklabel = "Service"},
                new TypeOfWork { worklabel = "Factory"}
            }.ForEach(u => macheteDB.TypesOfWork.Add(u));

            new List<Person>
            {
                new Person { firstname1 = "Jimmy", 
                             lastname1="Carter", 
                             gender="M", 
                             datecreated=DateTime.Now, 
                             dateupdated=DateTime.Now,
                             Createdby = "MacheteInitializer",
                             Updatedby = "MacheteInitializer",
                             Worker=new Worker {RaceID=1, 
                                                height = "6ft 1in",
                                                weight = "225lbs",
                                                englishlevelID =1,
                                                dateinUSA = DateTime.Now,
                                                dateinseattle = DateTime.Now,
                                                maritalstatus = "S",
                                                incomeID=1,
                                                neighborhoodID=1,
                                                countryoforigin="USA",
                                                dwccardnum=12345,
                                                memberexpirationdate=DateTime.Now,
                                                insuranceexpiration=DateTime.Now,
                                                datecreated=DateTime.Now, 
                                                dateupdated=DateTime.Now,
                                                Createdby = "MacheteInitializer",
                                                Updatedby = "MacheteInitializer"
                              }
                },
                new Person {firstname1 = "Arielle", 
                            lastname1="Rosenberg", 
                            gender="F",
                            datecreated=DateTime.Now, 
                            dateupdated=DateTime.Now, 
                            Createdby = "MacheteInitializer",
                            Updatedby = "MacheteInitializer",
                            Worker=new Worker {RaceID=1, 
                                                height = "12ft 1in",
                                                weight = "140lbs",
                                                englishlevelID =1,
                                                dateinUSA = DateTime.Now,
                                                dateinseattle = DateTime.Now,
                                                maritalstatus = "S",
                                                incomeID=1,
                                                neighborhoodID=1,
                                                countryoforigin="USA",
                                                dwccardnum=12346,
                                                memberexpirationdate=DateTime.Now,
                                                insuranceexpiration=DateTime.Now,
                                                datecreated=DateTime.Now, 
                                                dateupdated=DateTime.Now,
                                                Createdby = "MacheteInitializer",
                                                Updatedby = "MacheteInitializer"
                            }
                        },
                new Person {firstname1 = "Cariño", 
                            lastname1="Barragan", 
                            gender="F",
                            datecreated=DateTime.Now, 
                            dateupdated=DateTime.Now,
                            Createdby = "MacheteInitializer",
                            Updatedby = "MacheteInitializer"
                }
            }.ForEach(u => macheteDB.Persons.Add(u));
        }
    }
}
