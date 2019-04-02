
--up
update dbo.Lookups set [key] = [ltrCode] where category = 'worktype'

--down
update dbo.Lookups set [key] = null where category = 'worktype'
