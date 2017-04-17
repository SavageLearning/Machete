namespace Machete.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class api_refactor : DbMigration
    {
        public override void Up()
        {
            // Activities
            AddColumn("dbo.Activities", "nameEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "nameES", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "typeEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Activities", "typeES", c => c.String(maxLength: 50));
            Sql(@"update activities 
                    set
                      activities.nameEN = l.text_EN,
                      activities.nameES = l.text_ES,
                      activities.typeEN = ll.text_EN,
                      activities.typeEs = ll.text_ES
                    from 
                      Activities a
                      join lookups l on l.id = a.name
                      join lookups ll on ll.id = a.type");
            // Persons
            AddColumn("dbo.Persons", "fullName", c => c.String());
            Sql(@"update persons
                    set fullName = isnull(p.firstname1+' ','') +
		                     isnull(p.firstname2+' ','') +
		                     isnull(p.lastname1+' ','') +
		                     isnull(p.lastname2,'') 
                    from persons p");
            // Events
            AddColumn("dbo.Events", "eventTypeEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Events", "eventTypeES", c => c.String(maxLength: 50));
            Sql(@"update events 
                    set
                      events.eventTypeEN = l.text_EN,
                      events.eventTypeES = l.text_ES
                    from 
                      events e
                      join lookups l on l.id = e.eventType");
            // Workers
            AddColumn("dbo.Workers", "typeOfWork", c => c.String());
            AddColumn("dbo.Workers", "memberStatusEN", c => c.String(maxLength: 50));
            AddColumn("dbo.Workers", "memberStatusES", c => c.String(maxLength: 50));
            AddColumn("dbo.Workers", "fullNameAndID", c => c.String(maxLength: 100));
            AddColumn("dbo.Workers", "skillCodes", c => c.String());
            Sql(@"update workers 
                    set
                      workers.memberStatusEN = l.text_EN,
                      workers.memberStatusES = l.text_ES
                    from 
                      workers w
                      join lookups l on l.id = w.memberStatus");
            // separating queries because of join vs left join
            Sql(@"update workers
                    set workers.skillCodes =  
                      'E'+convert(varchar(2), w.englishlevelID)+' '+
                      isnull(l1.ltrcode+convert(varchar(2),l1.level)+' ','')+
                      isnull(l2.ltrcode+convert(varchar(2),l2.level)+' ','')+
                      isnull(l3.ltrcode+convert(varchar(2),l3.level),'')

                    from workers w
                    left join lookups l1 on l1.id = w.skill1
                    left join lookups l2 on l2.id = w.skill2
                    left join lookups l3 on l3.id = w.skill3");
            Sql(@"update workers 
                    set
                      workers.typeOfWork = l.ltrCode
                    from 
                      workers w
                      join lookups l on l.id = w.typeOfWorkID");
            Sql(@"update workers
                    set workers.fullNameAndID = 
                    convert(varchar(5), w.dwccardnum) + ' ' + p.fullName
                    FROM dbo.Workers w
                    join dbo.Persons p on p.id = w.id");
            // WorkOrders; needs to execute before WorkAssignments
            AddColumn("dbo.WorkOrders", "statusEN", c => c.String(maxLength: 50));
            AddColumn("dbo.WorkOrders", "statusES", c => c.String(maxLength: 50));
            AddColumn("dbo.WorkOrders", "transportMethodEN", c => c.String());
            AddColumn("dbo.WorkOrders", "transportMethodES", c => c.String());
            Sql(@"update workorders 
                    set
                      workorders.statusEN = l.text_EN,
                      workorders.statusES = l.text_ES,
                      workorders.transportMethodEN = ll.text_EN,
                      workorders.transportMethodES = ll.text_ES
                    from 
                      workorders wo
                      join lookups l on l.id = wo.status
                      join lookups ll on ll.id = wo.transportMethodID");
            Sql(@"update workorders
                    set paperOrderNum = ID
                    where paperOrderNum is null");
            // WorkAssignments
            AddColumn("dbo.WorkAssignments", "skillEN", c => c.String());
            AddColumn("dbo.WorkAssignments", "skillES", c => c.String());
            AddColumn("dbo.WorkAssignments", "fullWAID", c => c.String());
            AddColumn("dbo.WorkAssignments", "minEarnings", c => c.Double(nullable: false));
            AddColumn("dbo.WorkAssignments", "maxEarnings", c => c.Double(nullable: false));
            Sql(@"update workassignments 
                    set
                      workassignments.skillEN = l.text_EN,
                      workassignments.skillES = l.text_ES
                    from 
                      workassignments wa
                      join lookups l on l.id = wa.skillID");
            Sql(@"update WorkAssignments 
                    set minEarnings = (wa.days * wa.surcharge) + (wa.hourlyWage * wa.hours * wa.days),
                        maxEarnings = case when wa.hourRange is not null 
						                      then (wa.days * wa.surcharge) + (wa.hourlyWage * wa.hourRange * wa.days)
						                      else 0
						                      end
                    from WorkAssignments wa");
            Sql(@"update workassignments 
                    set fullWAID = REPLICATE('0',5-LEN(RTRIM(wo.paperOrderNum))) + RTRIM(wo.paperOrderNum) + '-' +
			                       REPLICATE('0',2-LEN(RTRIM(wa.pseudoID))) + RTRIM(wa.pseudoID) 
                    from WorkAssignments wa
                    join workorders wo on wo.id = wa.workOrderID
                    ");
            AddColumn("dbo.Lookups", "active", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lookups", "active");
            DropColumn("dbo.WorkOrders", "transportMethodES");
            DropColumn("dbo.WorkOrders", "transportMethodEN");
            DropColumn("dbo.WorkOrders", "statusES");
            DropColumn("dbo.WorkOrders", "statusEN");
            DropColumn("dbo.WorkAssignments", "maxEarnings");
            DropColumn("dbo.WorkAssignments", "minEarnings");
            DropColumn("dbo.WorkAssignments", "fullWAID");
            DropColumn("dbo.WorkAssignments", "skillES");
            DropColumn("dbo.WorkAssignments", "skillEN");
            DropColumn("dbo.Workers", "skillCodes");
            DropColumn("dbo.Workers", "memberStatusES");
            DropColumn("dbo.Workers", "memberStatusEN");
            DropColumn("dbo.Workers", "typeOfWork");
            DropColumn("dbo.Workers", "fullNameAndID");
            DropColumn("dbo.Events", "eventTypeES");
            DropColumn("dbo.Events", "eventTypeEN");
            DropColumn("dbo.Persons", "fullName");
            DropColumn("dbo.Activities", "typeES");
            DropColumn("dbo.Activities", "typeEN");
            DropColumn("dbo.Activities", "nameES");
            DropColumn("dbo.Activities", "nameEN");
        }
    }
}
