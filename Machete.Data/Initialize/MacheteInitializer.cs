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
            MacheteLookup.Initialize(macheteDB);
            new List<Person>
            {
                new Person { firstname1 = "Jimmy", 
                             lastname1="Carter", 
                             gender="M",
                             active = true, //true
                             datecreated=DateTime.Now, 
                             dateupdated=DateTime.Now, 
                             Createdby = "MacheteInitializer",
                             Updatedby = "MacheteInitializer",
                             Worker=new Worker {RaceID=1, 
                                                active = true,
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
                              },
                },
                new Person {firstname1 = "Arielle", 
                            lastname1="Rosenberg",
                            active = false,
                            gender="F",
                            datecreated=DateTime.Now, 
                            dateupdated=DateTime.Now, 
                            Createdby = "MacheteInitializer",
                            Updatedby = "MacheteInitializer",
                            Worker=new Worker {RaceID=1, 
                                                active = false,
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
                            active = true,
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
