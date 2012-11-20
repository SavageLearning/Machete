use machete
set identity_insert lookups on
go
insert into machete.dbo.lookups (id, category,text_EN, text_ES,selected,speciality,datecreated,dateupdated)
values (0, 'null','','',0,0,getdate(),GETDATE())
set identity_insert lookups off
go