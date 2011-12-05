using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Machete.Domain;
using System.Collections.ObjectModel;

namespace Machete.Data
{
    public class TestInitializer : DropCreateDatabaseAlways<MacheteContext>
    {
        protected override void Seed(MacheteContext DB)
        {
            //Initialize Lookup tables with static data
            MacheteLookup.Initialize(DB);
            DB.SaveChanges();

            Person person = new Person
            {
                firstname1 = "barack",
                lastname1 = "obama",
                gender = MacheteLookup.getID(DB, "gender", "Male"),
                active = true, //true
                datecreated = DateTime.Now,
                dateupdated = DateTime.Now,
                Createdby = "MacheteInitializer",
                Updatedby = "MacheteInitializer"
            };
            Worker worker = new Worker
            {
                RaceID = MacheteLookup.getID(DB, "race", "Latino"),
                active = true,
                typeOfWorkID = 20,
                dateOfMembership = DateTime.Now,
                dateOfBirth = DateTime.Now,
                height = "6ft 1in",
                weight = "225lbs",
                englishlevelID = 3,
                dateinUSA = DateTime.Now,
                dateinseattle = DateTime.Now,
                maritalstatus = MacheteLookup.getID(DB, "maritalstatus", "Single"),
                incomeID = MacheteLookup.getID(DB, "income", "Less than $10,000"),
                neighborhoodID = MacheteLookup.getID(DB, "neighborhood", "Seattle"),
                countryoforiginID = MacheteLookup.getID(DB, "countryoforigin", "USA"),
                dwccardnum = 12345,
                memberexpirationdate = DateTime.Now,
                insuranceexpiration = DateTime.Now,
                datecreated = DateTime.Now,
                dateupdated = DateTime.Now,
                Createdby = "MacheteInitializer",
                Updatedby = "MacheteInitializer"
            };
            Employer employer = new Employer
            {
                name = "Willy Wonka",
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
            };
            WorkOrder order = new WorkOrder
            {
                contactName = "Umpa Lumpa",
                status = 42,
                workSiteAddress1 = "Mayor's Office",
                workSiteAddress2 = "P.O. Box 94749",
                typeOfWorkID = 20,
                city = "Seattle",
                state = "WA",
                zipcode = "98124-4749",
                phone = "206-684-4000",
                paperOrderNum = 12042,
                timeFlexible = false,
                permanentPlacement = false,
                englishRequired = false,
                lunchSupplied = false,
                transportMethodID = 29,
                transportFee = 0,
                transportFeeExtra = 0,
                englishRequiredNote = "",
                description = "descriptiong string",
                dateTimeofWork = DateTime.Parse("08-10-2011 00:00:00"),
                datecreated = DateTime.Now,             //datetime
                dateupdated = DateTime.Now,              //datetime
                Createdby = "initialization script",
                Updatedby = "initialization script"
            };
            WorkAssignment assignment = new WorkAssignment
            {
                active = true,
                pseudoID = 1,
                description = "init script",
                englishLevelID = 2,
                skillID = 69,
                hourlyWage = 15,
                hours = 5,
                days = 1,
                datecreated = DateTime.Now,             //datetime
                dateupdated = DateTime.Now,              //datetime
                Createdby = "initialization script",
                Updatedby = "initialization script"
            };
            WorkerSignin signin = new WorkerSignin
            {
                dwccardnum = 30040,
                dateforsignin = DateTime.Parse("8/10/2011"),
                datecreated = DateTime.Now,             //datetime
                dateupdated = DateTime.Now,              //datetime
                Createdby = "initialization script",
                Updatedby = "initialization script"
            };
            WorkerRequest request = new WorkerRequest
            {
                datecreated = DateTime.Now,             //datetime
                dateupdated = DateTime.Now,              //datetime
                Createdby = "initialization script",
                Updatedby = "initialization script"
            };
            var dt = DateTime.Parse("8/10/2011");
            Person p1 = (Person)person.Clone(); DB.Persons.Add(p1); p1.Worker = (Worker)worker.Clone(); p1.Worker.dwccardnum = 30040; p1.Worker.skill1 = 62;
            Person p2 = (Person)person.Clone(); DB.Persons.Add(p2); p2.Worker = (Worker)worker.Clone(); p2.Worker.dwccardnum = 30041;
            Person p3 = (Person)person.Clone(); DB.Persons.Add(p3); p3.Worker = (Worker)worker.Clone(); p3.Worker.dwccardnum = 30042;
            DB.SaveChanges();
            WorkerSignin wsi1 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi1); wsi1.dwccardnum = 30040; wsi1.dateforsignin = dt; wsi1.WorkerID = 1;
            WorkerSignin wsi2 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi2); wsi2.dwccardnum = 30041; wsi2.dateforsignin = dt; wsi2.WorkerID = 2;
            WorkerSignin wsi3 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi3); wsi3.dwccardnum = 30042; wsi3.dateforsignin = dt; wsi3.WorkerID = 3;
            WorkerSignin wsi4 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi4); wsi4.dwccardnum = 30043; wsi4.dateforsignin = dt;
            WorkerSignin wsi5 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi5); wsi5.dwccardnum = 30044; wsi5.dateforsignin = dt; 
            DB.SaveChanges();
            Employer em1 = (Employer)employer.Clone(); DB.Employers.Add(em1);
            Employer em2 = (Employer)employer.Clone(); DB.Employers.Add(em2);
            Employer em3 = (Employer)employer.Clone(); DB.Employers.Add(em3);
            DB.SaveChanges();
            
                WorkOrder em1od1 = (WorkOrder)order.Clone();
                em1od1.Employer = em1;
                em1od1.paperOrderNum = 12420;
                em1od1.contactName = "Umpa1";
                em1od1.status = 42;
                WorkerRequest od1wr1 = (WorkerRequest)request.Clone();
                
                DB.WorkOrders.Add(em1od1); //CreateWorkOrder
                em1od1.workerRequests = new Collection<WorkerRequest>();
                DB.SaveChanges();
                od1wr1.workerRequested = p2.Worker;
                od1wr1.workOrder = em1od1;


                em1od1.workerRequests.Add(od1wr1);
                // em1.WorkOrders.Add(em1od1);
                DB.SaveChanges();
            WorkOrder em2od1 = (WorkOrder)order.Clone(); em2od1.Employer = em2; em2od1.paperOrderNum = 12421; em2od1.contactName = "Umpa2"; em2od1.status = 43; DB.WorkOrders.Add(em2od1);
            WorkOrder em2od2 = (WorkOrder)order.Clone(); em2od2.Employer = em2; em2od2.paperOrderNum = 12422; em2od2.contactName = "Umpa3"; em2od2.status = 44; DB.WorkOrders.Add(em2od2);
            WorkOrder em3od1 = (WorkOrder)order.Clone(); em3od1.Employer = em3; em3od1.paperOrderNum = 12423; em3od1.contactName = "Umpa4"; em3od1.status = 45; DB.WorkOrders.Add(em3od1);
            WorkOrder em3od2 = (WorkOrder)order.Clone(); em3od2.Employer = em3; em3od2.paperOrderNum = 12424; em3od2.contactName = "Umpa5"; em3od2.status = 44; DB.WorkOrders.Add(em3od2);
            WorkOrder em3od3 = (WorkOrder)order.Clone(); em3od3.Employer = em3; em3od3.paperOrderNum = 12425; em3od3.contactName = "Umpa6"; em3od3.status = 42; DB.WorkOrders.Add(em3od3);
            DB.SaveChanges();
            WorkAssignment em1od1as1 = (WorkAssignment)assignment.Clone(); em1od1as1.workOrder = em1od1;
            em1od1as1.description = "foostring1";
            em1od1as1.Updatedby = "foostring2";
            em1od1as1.workerAssignedID = 1;
            em1od1as1.skillID = 70; DB.WorkAssignments.Add(em1od1as1);

            em1od1.dateTimeofWork = DateTime.Parse("08-10-2011 09:00:00");
            WorkAssignment em1od1as2 = (WorkAssignment)assignment.Clone(); em1od1as2.workOrder = em1od1; DB.WorkAssignments.Add(em1od1as2);
            em1od1as2.workerAssignedID = 2;
            em1od1as2.skillID = 61;
            em1od1as2.englishLevelID = 2;
            WorkAssignment em1od1as3 = (WorkAssignment)assignment.Clone(); em1od1as3.workOrder = em1od1; DB.WorkAssignments.Add(em1od1as3);
            em1od1as1.workerAssignedID = 1;
            em1od1as3.workerAssignedID = 3;
            //
            WorkAssignment em2od1as1 = (WorkAssignment)assignment.Clone(); em2od1as1.workOrder = em2od1; DB.WorkAssignments.Add(em2od1as1);
            em2od1as1.workerAssignedID = 1;
            WorkAssignment em2od1as2 = (WorkAssignment)assignment.Clone(); em2od1as2.workOrder = em2od1; DB.WorkAssignments.Add(em2od1as2);
            em2od1as2.workerAssignedID = 2;
            //
            WorkAssignment em2od2as1 = (WorkAssignment)assignment.Clone(); em2od2as1.workOrder = em2od2; DB.WorkAssignments.Add(em2od2as1);
            em2od2as1.workerAssignedID = 1;
            WorkAssignment em2od2as2 = (WorkAssignment)assignment.Clone(); em2od2as2.workOrder = em2od2; DB.WorkAssignments.Add(em2od2as2);
            em2od2as2.workerAssignedID = 2;
            //
            WorkAssignment em3od1as1 = (WorkAssignment)assignment.Clone(); em3od1as1.workOrder = em3od1; DB.WorkAssignments.Add(em3od1as1);
            em3od1as1.workerAssignedID = 1;
            //
            WorkAssignment em3od2as1 = (WorkAssignment)assignment.Clone(); em3od2as1.workOrder = em3od2; DB.WorkAssignments.Add(em3od2as1);
            em3od2as1.workerAssignedID = 1;
            //
            WorkAssignment em3od3as1 = (WorkAssignment)assignment.Clone(); em3od3as1.workOrder = em3od3; DB.WorkAssignments.Add(em3od3as1);

            DB.SaveChanges();
        }
    }
}
