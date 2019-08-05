select    
      sum(reserved_page_count) * 8.0 / 1024 [SizeInMB]
from    
      sys.dm_db_partition_stats

GO

select    
      sys.objects.name, sum(reserved_page_count) * 8.0 / 1024 [SizeInMB]
from    
      sys.dm_db_partition_stats, sys.objects
where    
      sys.dm_db_partition_stats.object_id = sys.objects.object_id

group by sys.objects.name
order by sum(reserved_page_count) DESC

------------------------

select column_name, is_nullable, data_type, character_maximum_length
from machete_casa_test.information_schema.columns
where TABLE_NAME = 'Workers'
order by table_name, ordinal_position
