
select * from employers where datecreated < '2010-01-01' or dateupdated < '2010-01-01'
select * from Employers where ID = 2638 or ID = 2986 or ID = 3762 or ID = 3763 or ID = 3764 or ID = 3774 or ID = 3780 
--update Employers set dateupdated = '2012-12-08' where ID = 2638
--update Employers set dateupdated = '2012-12-08' where ID = 2986
--update Employers set dateupdated = '2012-12-08', datecreated = '2012-12-08' where ID = 3762
--update Employers set dateupdated = '2012-12-08', datecreated = '2012-12-08' where ID = 3763 or ID = 3764
--update Employers set dateupdated = '2012-12-15' where ID = 3774
--update Employers set dateupdated = '2012-12-17' where ID = 3780

select * from events where datecreated < '2010-01-01' or dateupdated < '2010-01-01'
select * from events where ID = 81 or ID = 82
--update Events set dateFrom = '2012-11-11', dateTo = '2012-11-11', datecreated = '2012-11-11', dateupdated =  '2012-11-11' where ID = 81 or ID = 82

select ID, dateTimeofWork, datecreated, dateupdated from WorkOrders where datecreated < '2010-01-01' or dateupdated < '2010-01-01'
order by id
--IDs 8530, 8583, 8592, 8631, 8634, 8635, 8637, 8638, 8639, 8640, 8641, 8642, 8643, 8644, 8663, 8670, 8679, 8680, 8681, 8683, 8685, 8686, 8695, 8696, 8699, 8700, 8711, 8713, 8715, 8718, 8719, 8723
select * from WorkOrders where ID between 8529 and 8531
--update WorkOrders set dateTimeofWork = '2012-11-27 09:00:00.000', dateupdated = '2012-11-27 13:56:38.907' where id = 8530
select * from WorkOrders where ID between 8582 and 8584
--update WorkOrders set dateupdated = '2012-12-03 09:14:18.810' where id = 8583
select * from WorkOrders where ID between 8591 and 8593
--update WorkOrders set dateupdated = '2012-12-15' where id = 8592
select * from WorkOrders where ID between 8630 and 8632
--update WorkOrders set datecreated = '2012-12-08' where id = 8631
--update WorkOrders set datecreated = '2012-12-08' where id = 8634 or ID = 8635 or ID between 8637 and 8644
--update WorkOrders set datecreated = '2012-12-15' where id = 8699 or ID = 8696 or ID = 8700
--update WorkOrders set datecreated = '2012-12-18 12:46:27.037' where ID = 8723
--update WorkOrders set dateupdated = '2012-12-15' where id = 8663 or id = 8679 or id = 8680 or id = 8681 or id = 8683 or id = 8685 or id = 8686 
--update WorkOrders set dateupdated = '2012-12-17' where id = 8670 or id = 8695 or id = 8711 or id = 8713 or id = 8715 or id = 8718 or id = 8719 

select wo.id, wo.datecreated, wa.workorderid, wa.datecreated 
from workassignments wa 
join workorders wo on (wo.id = wa.workorderid)
where wa.datecreated < '2010-01-01'

--update workassignments
--set datecreated = wo.datecreated
--from workassignments wa
--join workorders wo on (wo.id = wa.workorderid)
--where wa.datecreated < '2010-01-01'

select wo.id, wo.dateupdated, wa.workorderid, wa.dateupdated 
from workassignments wa 
join workorders wo on (wo.id = wa.workorderid)
where wa.dateupdated < '2010-01-01'

--update workassignments
--set dateupdated = wo.dateupdated
--from workassignments wa
--join workorders wo on (wo.id = wa.workorderid)
--where wa.dateupdated < '2010-01-01'

select wr.id, wo.id, wo.datecreated, wr.datecreated from workerrequests wr 
join workorders wo on (wo.id = wr.workorderid)
where wr.datecreated < '2010-01-01' or wr.dateupdated < '2010-01-01'

--update workerrequests
--set dateupdated = wo.dateupdated
--from workerrequests wr
--join workorders wo on (wo.id = wr.workorderid)
--where wr.dateupdated < '2010-01-01'

--update workerrequests
--set datecreated = wo.datecreated
--from workerrequests wr
--join workorders wo on (wo.id = wr.workorderid)
--where wr.datecreated < '2010-01-01'

select * from WorkerSignins where datecreated < '2010-01-01' or dateupdated < '2010-01-01' 

select * from WorkerSignins where ID between 46474 and 46545
select * from workersignins where ID >= 46473
--i can tell from looking at the select above (>= 46473) that they re-entered the data on the 10th
-- so im deleting the original signins
--delete from WorkerSignins where ID between 46474 and 46545

--eyeballing workersignins...2 w/ dateforsignin wrong
--update workersignins
--set dateforsignin = '2012-12-15 00:00:00.000'
--where id = 47017 or id = 47018

select id, dateforsignin, datecreated  from workersignins where id between 46944 and 47018

--update workersignins
--set datecreated =  dateadd(second, 219423000,datecreated)  
--from workersignins 
--where id between 46944 and 47018

--update workersignins
--set dateupdated = datecreated
--from workersignins 
--where id between 46944 and 47018

--update workersignins
--set dateupdated = datecreated
--from workersignins 
--where id = 47024 or id = 47030 or  id = 47042 or id = 47048 or id = 47052 or id=47056

--update workersignins
--set dateupdated = '2012-12-17 05:14:05.483', datecreated = '2012-12-17 05:14:05.483'
--from workersignins 
--where id = 47026 or id = 47036 or id = 47069 or id = 47070 or id = 47071
