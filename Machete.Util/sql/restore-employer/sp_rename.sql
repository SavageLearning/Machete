
sp_rename 'Employers', 'Employers2018'; GO
sp_rename 'WorkOrders', 'WorkOrders2018'; GO
sp_rename 'WorkAssignments', 'WorkAssignments2018'; GO

select * from dbo.Employers2018 where id = 2002
select * from dbo.WorkOrders2018 where EmployerID = 2002
select wa.* from dbo.WorkAssignments2018 wa join dbo.WorkOrders2018 wo on wa.workOrderID = wo.ID where wo.EmployerID = 2002

