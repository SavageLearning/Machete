using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;

namespace Machete.Data
{
    public class MacheteInitializer : DropCreateDatabaseIfModelChanges<MacheteContext>
    //public class MacheteInitializer : CreateDatabaseIfNotExists<MacheteContext>
    {
        
        protected override void Seed(MacheteContext DB)
        {
            //Initialize Lookups
            MacheteLookup.Initialize(DB);
            DB.SaveChanges();
            
            new List<Person>
            {
                new Person { firstname1 = "Jimmy", 
                             lastname1="Carter", 
                             //gender=DB.Lookups.Single(s => s.category =="gender" && s.text_EN =="Male").ID,
                             gender=MacheteLookup.getID(DB, "gender", "Male"),
                             active = true, //true
                             datecreated=DateTime.Now, 
                             dateupdated=DateTime.Now, 
                             Createdby = "MacheteInitializer",
                             Updatedby = "MacheteInitializer",
                             Worker=new Worker {RaceID=MacheteLookup.getID(DB, "race", "Latino"), 
                                                active = true,
                                                dateOfMembership = DateTime.Now,
                                                dateOfBirth = DateTime.Now,
                                                height = "6ft 1in",
                                                weight = "225lbs",
                                                englishlevelID =MacheteLookup.getID(DB, "language", "English-conversational"),
                                                dateinUSA = DateTime.Now,
                                                dateinseattle = DateTime.Now,
                                                maritalstatus = MacheteLookup.getID(DB, "maritalstatus", "Single"),
                                                incomeID=MacheteLookup.getID(DB, "income", "Less than $10,000"),
                                                neighborhoodID=MacheteLookup.getID(DB, "neighborhood", "Seattle"),
                                                countryoforiginID=MacheteLookup.getID(DB, "countryoforigin", "USA"),
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
                            gender=MacheteLookup.getID(DB, "gender", "Female"),
                            datecreated=DateTime.Now, 
                            dateupdated=DateTime.Now, 
                            Createdby = "MacheteInitializer",
                            Updatedby = "MacheteInitializer",
                            Worker=new Worker {RaceID=MacheteLookup.getID(DB, "race", "Caucasian"), 
                                                active = false,
                                                dateOfMembership = DateTime.Now,
                                                dateOfBirth = DateTime.Now,
                                                height = "12ft 1in",
                                                weight = "140lbs",
                                                englishlevelID =1,
                                                dateinUSA = DateTime.Now,
                                                dateinseattle = DateTime.Now,
                                                maritalstatus = MacheteLookup.getID(DB, "maritalstatus", "Single"),
                                                incomeID=MacheteLookup.getID(DB, "income", "Less than $10,000"),
                                                neighborhoodID=MacheteLookup.getID(DB, "neighborhood", "Seattle"),
                                                countryoforiginID=MacheteLookup.getID(DB, "countryoforigin", "USA"),
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
                            gender=MacheteLookup.getID(DB, "gender", "Female"),
                            datecreated=DateTime.Now, 
                            dateupdated=DateTime.Now,
                            Createdby = "MacheteInitializer",
                            Updatedby = "MacheteInitializer"
                }
            }.ForEach(u => DB.Persons.Add(u));
            DB.SaveChanges();
            new List<Employer>
            {
                new Employer {  name = "Savage Learning, LLC",
                                active = true,
                                address1 = "2410 Boyer Ave E",
                                address2 = "Apt 310",
                                city = "seattle",
                                state = "wa",
                                zipcode = "98112",
                                phone = "206-660-3361",
                                cellphone = "123-456-7890",
                                email = "jimmy@savagelearning.com",
                                referredby = MacheteLookup.getID(DB, "emplrreference", "Flyer"),
                                datecreated = DateTime.Now,             //datetime
                                dateupdated = DateTime.Now,              //datetime
                                Createdby = "initialization script",
                                Updatedby = "initialization script"
                },
                new Employer {             
                                name = "Casa Latina",
                                active = true,
                                address1 = "317 17th Ave S",
                                address2 = null,
                                city = "Seattle",
                                state = "WA",
                                zipcode = "98144",
                                phone = "206.956.0779",
                                cellphone = "123-456-7890",
                                email = "info@casa-latina.org",
                                referredby = MacheteLookup.getID(DB, "emplrreference", "Facebook"),
                                datecreated = DateTime.Now,             //datetime
                                dateupdated = DateTime.Now,              //datetime
                                Createdby = "initialization script",
                                Updatedby = "initialization script"
                },
                new Employer {             
                                name = "Mike McGinn",
                                active = false,
                                address1 = "Mayor's Office",
                                address2 = "P.O. Box 94749",
                                city = "Seattle",
                                state = "WA",
                                zipcode = "98124-4749",
                                phone = "206-684-4000",
                                cellphone = "123-456-7890",
                                referredby = null,
                                datecreated = DateTime.Now,             //datetime
                                dateupdated = DateTime.Now,              //datetime
                                Createdby = "initialization script",
                                Updatedby = "initialization script"
                },
            }.ForEach(u => DB.Employers.Add(u));
        }
    }
}
