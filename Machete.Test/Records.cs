using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;
using System.Collections.ObjectModel;

namespace Machete.Test
{
    public class Records
    {
        #region Workers
        public static Worker worker = new Worker
        {   
            typeOfWorkID = 20,
            RaceID = MacheteLookup.cache.First(x => x.category == "race" && x.text_EN =="Latino").ID,                     //byte
            raceother = "Records._worker1", //string
            height = "too tall",            //string
            weight = "too big",             //string
            englishlevelID = 1,             //byte
            recentarrival = true,           //bool
            dateinUSA = DateTime.Now,       //datetime
            dateinseattle = DateTime.Now,   //datetime
            disabled = true,                //bool
            disabilitydesc = "foo",         //string
            maritalstatus = MacheteLookup.cache.First(x => x.category == "maritalstatus" && x.text_EN == "Single").ID,            //string
            livewithchildren = true,        //bool
            numofchildren = 0,              //byte
            incomeID = MacheteLookup.cache.First(x => x.category == "income" && x.text_EN == @"Less than $15,000").ID,                   //byte
            livealone = true,               //bool
            emcontUSAname = "Bill Clinton", //string
            emcontUSAphone = "1234567890",  //string
            emcontUSArelation = "idol",     //string
            dwccardnum = 12345,             //int
            neighborhoodID = MacheteLookup.cache.First(x => x.category == "neighborhood" && x.text_EN == "Seattle").ID,             //byte
            immigrantrefugee = true,        //bool
            countryoforiginID = MacheteLookup.cache.First(x => x.category == "countryoforigin" && x.text_EN == "USA").ID,        //string
            emcontoriginname = "Barak Obama",   //string
            emcontoriginphone = "1234567890",   //string
            emcontoriginrelation = "friend",    //string
            memberexpirationdate = DateTime.Now,    //datetime
            driverslicense = true,                  //bool
            licenseexpirationdate = DateTime.Now,   //datetime
            carinsurance = true,                    //bool
            insuranceexpiration = DateTime.Now,     //datetime
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script",
            dateOfBirth = DateTime.Now,
            dateOfMembership = DateTime.Now
        };
        public static Worker _worker1 = (Worker)worker.Clone();
        public static Worker _worker2 = (Worker)worker.Clone();
        public static Worker _worker3 = (Worker)worker.Clone();
        #endregion
        #region Persons
        public static Person person = new Person
        {
            firstname1 = "barack",
            lastname1 = "obama",
            gender = MacheteLookup.cache.First(x => x.category == "gender" && x.text_EN == "Male").ID,
            active = true, //true
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        #endregion
        #region Employers
        public static Employer employer = new Employer
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
            referredby = MacheteLookup.cache.First(c => c.category == "emplrreference" && c.text_EN == "Facebook").ID,
            email = "willy@wonka.com",
            notes = "A note!",
            referredbyOther = "another reference",
            blogparticipate = true,
            business = true,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        public static Employer _employer1 = (Employer)employer.Clone();
        public static Employer _employer2 = (Employer)employer.Clone();
        public static Employer _employer3 = (Employer)employer.Clone();
        #endregion
        #region WorkOrders
        public static WorkOrder order = new WorkOrder
        {
            contactName = "Umpa Lumpa",
            workSiteAddress1 = "2400 Main Ave E",
            workSiteAddress2 = "Apt 207",
            status = 42,
            city = "london",
            state = "uk",
            zipcode = "12345",
            phone = "123-456-7890",
            typeOfWorkID = 20,
            englishRequired = false,
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            englishRequiredNote = "",
            description = "descriptiong string",
            dateTimeofWork = DateTime.Today,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static WorkOrder _workOrder1 = (WorkOrder)order.Clone();
        public static WorkOrder _workOrder2 = (WorkOrder)order.Clone();
        public static WorkOrder _workOrder3 = (WorkOrder)order.Clone();
        public static WorkOrder _workOrder4 = (WorkOrder)order.Clone();
        public static WorkOrder _workOrder5 = (WorkOrder)order.Clone();
        public static WorkOrder _workOrder6 = (WorkOrder)order.Clone();
        
        #endregion

        #region workassignments
        public static WorkAssignment assignment = new WorkAssignment
        {
            //ID = 1,
            active = true,
            days = 1,
            hours = 5,
            //chambita = false,
            hourlyWage = 20.50,
            skillID = 69,
            description = "init script",
            englishLevelID = 1,
            //evaluation
            qualityOfWork = 5,
            followDirections = 4,
            attitude = 4,
            reliability = 5,
            transportProgram = 3,
            comments = "I really like to make comments.",
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };
        public static WorkAssignment _workAssignment1 = (WorkAssignment)assignment.Clone();
        #endregion

        public static Activity activity = new Activity
        {
            name = 98,
            type = 90,
            teacher = "foo",
            notes = "foo too",
            dateStart = DateTime.Now,
            dateEnd = DateTime.Now,
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        public static ActivitySignin activitysignin = new ActivitySignin
        {

            dateforsignin = DateTime.Now,            
            datecreated = DateTime.Now,
            dateupdated = DateTime.Now,
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };

        public static WorkerSignin signin = new WorkerSignin
        {
            dwccardnum = 30040,
            dateforsignin = DateTime.Today,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };
        public static WorkerRequest request = new WorkerRequest
        {
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "TestInitializer",
            Updatedby = "TestInitializer"
        };



        public static void Initialize(MacheteContext DB)
        {
            var dt = DateTime.Today;
            #region INIT persons and workers

            Person p1 = (Person)person.Clone(); p1.Worker = (Worker)worker.Clone(); p1.Worker.dwccardnum = 30040; p1.Worker.skill1 = 62;
            p1.Worker.Person = p1;
            DB.Persons.Add(p1);
            DB.Workers.Add(p1.Worker);
            Person p2 = (Person)person.Clone(); DB.Persons.Add(p2); p2.Worker = (Worker)worker.Clone(); p2.Worker.dwccardnum = 30041;
            Person p3 = (Person)person.Clone(); DB.Persons.Add(p3); p3.Worker = (Worker)worker.Clone(); p3.Worker.dwccardnum = 30042;

            //DB.SaveChanges();
            WorkerSignin wsi1 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi1); wsi1.dwccardnum = 30040; wsi1.dateforsignin = dt; wsi1.WorkerID = 1;
            WorkerSignin wsi2 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi2); wsi2.dwccardnum = 30041; wsi2.dateforsignin = dt; wsi2.WorkerID = 2;
            WorkerSignin wsi3 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi3); wsi3.dwccardnum = 30042; wsi3.dateforsignin = dt; wsi3.WorkerID = 3;
            WorkerSignin wsi4 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi4); wsi4.dwccardnum = 30043; wsi4.dateforsignin = dt;
            WorkerSignin wsi5 = (WorkerSignin)signin.Clone(); DB.WorkerSignins.Add(wsi5); wsi5.dwccardnum = 30044; wsi5.dateforsignin = dt;
            DB.SaveChanges();
            #endregion
            #region INIT employers
            Employer em1 = (Employer)employer.Clone(); DB.Employers.Add(em1);
            Employer em2 = (Employer)employer.Clone(); DB.Employers.Add(em2);
            Employer em3 = (Employer)employer.Clone(); DB.Employers.Add(em3);
            DB.SaveChanges();
            #endregion
            #region INIT workorders
            WorkOrder em1od1 = (WorkOrder)order.Clone();
            em1od1.Employer = em1;
            em1od1.paperOrderNum = 12420;
            em1od1.contactName = "Umpa1";
            em1od1.status = 42;            
            DB.WorkOrders.Add(em1od1); //CreateWorkOrder
            DB.SaveChanges();
            WorkOrder em2od1 = (WorkOrder)order.Clone(); em2od1.Employer = em2; em2od1.paperOrderNum = 12421; em2od1.contactName = "Umpa2"; em2od1.status = 43; DB.WorkOrders.Add(em2od1);
            WorkOrder em2od2 = (WorkOrder)order.Clone(); em2od2.Employer = em2; em2od2.paperOrderNum = 12422; em2od2.contactName = "Umpa3"; em2od2.status = 44; DB.WorkOrders.Add(em2od2);
            WorkOrder em3od1 = (WorkOrder)order.Clone(); em3od1.Employer = em3; em3od1.paperOrderNum = 12423; em3od1.contactName = "Umpa4"; em3od1.status = 45; DB.WorkOrders.Add(em3od1);
            WorkOrder em3od2 = (WorkOrder)order.Clone(); em3od2.Employer = em3; em3od2.paperOrderNum = 12424; em3od2.contactName = "Umpa5"; em3od2.status = 44; DB.WorkOrders.Add(em3od2);
            WorkOrder em3od3 = (WorkOrder)order.Clone(); em3od3.Employer = em3; em3od3.paperOrderNum = 12425; em3od3.contactName = "Umpa6"; em3od3.status = 42; DB.WorkOrders.Add(em3od3);
            DB.SaveChanges();
            #endregion
            #region INIT workassignments
            WorkAssignment em1od1as1 = (WorkAssignment)assignment.Clone(); em1od1as1.workOrder = em1od1; DB.WorkAssignments.Add(em1od1as1);
            em1od1as1.description = "foostring1";
            em1od1as1.Updatedby = "foostring2";
            em1od1as1.workerAssignedID = 1;
            em1od1as1.skillID = 70; DB.WorkAssignments.Add(em1od1as1);

            em1od1.dateTimeofWork = DateTime.Parse(DateTime.Today.ToShortDateString() + " 09:00:00");
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
            WorkerRequest od3wr1 = (WorkerRequest)request.Clone();
            em3od3.workerRequests = new Collection<WorkerRequest>();
            od3wr1.workerRequested = p2.Worker;
            od3wr1.workOrder = em3od3;


            em3od3.workerRequests.Add(od3wr1);
            DB.SaveChanges(); 
            #endregion
            #region INIT ACTIVITIES
            Activity a1 = (Activity)activity.Clone(); DB.Activities.Add(a1);
            Activity a2 = (Activity)activity.Clone(); DB.Activities.Add(a2);
            Activity a3 = (Activity)activity.Clone(); DB.Activities.Add(a3);
            DB.SaveChanges();
            ActivitySignin a1as1 = (ActivitySignin)activitysignin.Clone(); DB.ActivitySignins.Add(a1as1); a1as1.ActivityID = 1; a1as1.dwccardnum = 30040; a1as1.dateforsignin = dt; a1as1.WorkerID = 1;
            DB.SaveChanges();
            #endregion
        }
    }
}
