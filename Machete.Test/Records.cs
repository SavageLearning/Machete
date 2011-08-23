using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using Machete.Data;

namespace Machete.Test
{
    public class Records
    {
        #region Workers
        public static Worker _worker1 = new Worker
        {   
            ID = 1,                         //#C data type
            RaceID = 1,                     //byte
            raceother = "Records._worker1", //string
            height = "too tall",            //string
            weight = "too big",             //string
            englishlevelID = 1,             //byte
            recentarrival = true,           //bool
            dateinUSA = DateTime.Now,       //datetime
            dateinseattle = DateTime.Now,   //datetime
            disabled = true,                //bool
            disabilitydesc = "foo",         //string
            maritalstatus = 1,            //string
            livewithchildren = true,        //bool
            numofchildren = 0,              //byte
            incomeID = 1,                   //byte
            livealone = true,               //bool
            emcontUSAname = "Bill Clinton", //string
            emcontUSAphone = "1234567890",  //string
            emcontUSArelation = "idol",     //string
            dwccardnum = 12345,             //int
            neighborhoodID = 1,             //byte
            immigrantrefugee = true,        //bool
            countryoforiginID = 1,        //string
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
            Updatedby = "initialization script"
        };
        public static Worker _worker2 = new Worker
        {                                   //#C data type
            ID =2 , 
            RaceID = 1,                     //byte
            raceother = "Records._worker2",         //string
            height = "too tall",            //string
            weight = "too big",             //string
            englishlevelID = 1,             //byte
            recentarrival = true,           //bool
            dateinUSA = DateTime.Now,       //datetime
            dateinseattle = DateTime.Now,   //datetime
            disabled = true,                //bool
            disabilitydesc = "foo",         //string
            maritalstatus = 1,            //string
            livewithchildren = true,        //bool
            numofchildren = 0,              //byte
            incomeID = 1,                   //byte
            livealone = true,               //bool
            emcontUSAname = "Bill Clinton", //string
            emcontUSAphone = "1234567890",  //string
            emcontUSArelation = "idol",     //string
            dwccardnum = 12346,                 //int
            neighborhoodID = 1,             //byte
            immigrantrefugee = true,        //bool
            countryoforiginID = 1,        //string
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
            Updatedby = "initialization script"
        };
        public static Worker _worker3 = new Worker
        {                                   //#C data type
            ID = 3,
            RaceID = 1,                     //byte
            raceother = "Records._worker3",         //string
            height = "too tall",            //string
            weight = "too big",             //string
            englishlevelID = 1,             //byte
            recentarrival = true,           //bool
            dateinUSA = DateTime.Now,       //datetime
            dateinseattle = DateTime.Now,   //datetime
            disabled = true,                //bool
            disabilitydesc = "foo",         //string
            maritalstatus = 1,            //string
            livewithchildren = true,        //bool
            numofchildren = 0,              //byte
            incomeID = 1,                   //byte
            livealone = true,               //bool
            emcontUSAname = "Bill Clinton", //string
            emcontUSAphone = "1234567890",  //string
            emcontUSArelation = "idol",     //string
            dwccardnum = 12347,                 //int
            neighborhoodID = 1,             //byte
            immigrantrefugee = true,        //bool
            countryoforiginID = 1,        //string
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
            Updatedby = "initialization script"
        };
        #endregion
        #region Persons
        public static Person _person1 = new Person
        {
            ID = 1,
            firstname1 = "Records._person1",
            firstname2 = "default record",
            lastname1 = "testestestest",
            address1 = "123 Foo St",
            city = "Foo town",
            state = "FO",
            zipcode = "12345",
            phone = "No phone",
            gender = 3,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };
        public static Person _person2 = new Person
        {
            ID = 2,
            firstname1 = "Records._person2",
            firstname2 = "default record",
            lastname1 = "testestestest",
            address1 = "123 Foo St",
            city = "Foo town",
            state = "FO",
            zipcode = "12345",
            phone = "No phone",
            gender = 3,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };
        public static Person _person3 = new Person
        {
            ID = 3,
            firstname1 = "Records._person3",
            firstname2 = "default record",
            lastname1 = "testestestest",
            address1 = "123 Foo St",
            city = "Foo town",
            state = "FO",
            zipcode = "12345",
            phone = "No phone",
            gender = 3,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };
        public static Person _person4 = new Person
        {
            ID = 4,
            firstname1 = "Records._person4",
            firstname2 = "default record",
            lastname1 = "testestestest",
            address1 = "123 Foo St",
            city = "Foo town",
            state = "FO",
            zipcode = "12345",
            phone = "No phone",
            gender = 3,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };
        public static Person _person5 = new Person
        {
            ID = 5,
            firstname1 = "Records._person5",
            firstname2 = "default record",
            lastname1 = "testestestest",
            address1 = "123 Foo St",
            city = "Foo town",
            state = "FO",
            zipcode = "12345",
            phone = "No phone",
            gender = 3,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };
        #endregion
        #region Employers
        public static Employer _employer1 = new Employer
        {
            //ID = 1,
            name = "Savage Learning, LLC",
            address1 = "1234 Street St",
            address2 = "Apt 1",
            city = "seattle",
            state = "wa",
            zipcode = "98112",
            phone = "206-123-1231",
            email = "jimmy@savagelearning.com",
            referredby = 1,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static Employer _employer2 = new Employer
        {
            //ID = 2,
            name = "Casa Latina",
            address1 = "317 17th Ave S",
            address2 = null,
            city = "Seattle",
            state = "WA",
            zipcode = "98144",
            phone = "206.956.0779",
            email = "info@casa-latina.org",
            referredby = 2,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static Employer _employer3 = new Employer
        {
            //ID = 3,
            name = "Mike McGinn",
            address1 = "Mayor's Office",
            address2 = "P.O. Box 94749",
            city = "Seattle",
            state = "WA",
            zipcode = "98124-4749",
            phone = "206-684-4000",
            referredby = null,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"

        };
        #endregion
        #region WorkOrders
        public static WorkOrder _workOrder1 = new WorkOrder
        {
            //ID = 1,
            EmployerID = 1,
            workSiteAddress1 = "2400 Boyer Ave E",
            workSiteAddress2 = "Apt 207",
            city = "",
            state = "",
            zipcode = "",
            phone = "",
            typeOfWorkID = 1,
            //dateTimeofWork = DateTime.Now,
            //timeFlexible = true,
            //hourlyWage = 14.50,
            //hours = 4,
            //hoursChambita = null,
            //days = 1,
            englishRequired = false,
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static WorkOrder _workOrder2 = new WorkOrder
        {
            //ID = 2,
            EmployerID = 1,
            workSiteAddress1 = "",
            workSiteAddress2 = "",
            city = "",
            state = "",
            zipcode = "",
            phone = "",
            typeOfWorkID = 1,
            //dateTimeofWork = DateTime.Now,
            //timeFlexible = true,
            //hourlyWage = 14.50,
            //hours = 4,
            //hoursChambita = null,
            //days = 1,
            englishRequired = false,
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static WorkOrder _workOrder3 = new WorkOrder
        {
            //ID = 3,
            EmployerID = 1,
            workSiteAddress1 = "",
            workSiteAddress2 = "",
            city = "",
            state = "",
            zipcode = "",
            phone = "",
            typeOfWorkID = 1,
            //dateTimeofWork = DateTime.Now,
            //timeFlexible = true,
            //hourlyWage = 14.50,
            //hours = 4,
            //hoursChambita = null,
            //days = 1,
            englishRequired = false,
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static WorkOrder _workOrder4 = new WorkOrder
        {
            //ID = 4,
            EmployerID = 2,
            workSiteAddress1 = "",
            workSiteAddress2 = "",
            city = "",
            state = "",
            zipcode = "",
            phone = "",
            typeOfWorkID = 1,
            //dateTimeofWork = DateTime.Now,
            //timeFlexible = true,
            //hourlyWage = 14.50,
            //hours = 4,
            //hoursChambita = null,
            //days = 1,
            englishRequired = false,
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static WorkOrder _workOrder5 = new WorkOrder
        {
            //ID = 5,
            EmployerID = 2,
            workSiteAddress1 = "",
            workSiteAddress2 = "",
            typeOfWorkID = 1,
            //dateTimeofWork = DateTime.Now,
            //timeFlexible = true,
            //hourlyWage = 14.50,
            //hours = 4,
            //hoursChambita = null,
            //days = 1,
            englishRequired = false,
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        public static WorkOrder _workOrder6 = new WorkOrder
        {
            //ID = 6,
            EmployerID = 3,
            workSiteAddress1 = "",
            workSiteAddress2 = "",
            city = "",
            state = "",
            zipcode = "",
            phone = "",
            typeOfWorkID = 1,
            //dateTimeofWork = DateTime.Now,
            //timeFlexible = true,
            //hourlyWage = 14.50,
            //hours = 4,
            //hoursChambita = null,
            //days = 1,
            englishRequired = true,
            englishRequiredNote = "Southern accent if possible.",
            description = "some description about the work order.",
            contactName = "Milton Friedman",
            status = 1, 
            lunchSupplied = false,
            permanentPlacement = false,
            transportMethodID = 1,
            transportFee = 20.75,
            transportFeeExtra = 0,
            datecreated = DateTime.Now,             //datetime
            dateupdated = DateTime.Now,              //datetime
            Createdby = "initialization script",
            Updatedby = "initialization script"
        };

        #endregion

        #region workassignments
        public static WorkAssignment _workAssignment1 = new WorkAssignment
        {
            //ID = 1,
            active = true,
            workerAssignedID = 1,
            
            workerSigninID = 1,
            workOrderID = 1,
            days = 1,
            hours = 5,
            //chambita = false,
            hourlyWage = 20.50,
            description = "A job I want done",
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
        #endregion

    }
}
