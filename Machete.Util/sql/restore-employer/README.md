Sometimes centers delete employers.  

We can restore those employers but it's kind of a painful process. Here's what you do:  

1. Restore an earlier copy of the database in Azure.
1. Connect to the restored database and run the sp_rename.sql script. Replace `id` and `EmployerID` with the value of the deleted employer. You can find the deletion action with the record ID in NLog. The `select` statements will give you the employer record, their work orders, and their work assignments as separate results.
1. Connect to the live database and run the restore_employer.sql script. You will need to provide the proper credentials in place of the empty strings. Also, you'll need to put the name of the restored database and the server name in place of the dummy values.

That's it! There's a little test `select` statement at the end of the restore_employer.sql script so that you can see the result of the `insert`. The rows returned should match the number of rows for work assignments in the final `select` statement from sp_rename.sql.
