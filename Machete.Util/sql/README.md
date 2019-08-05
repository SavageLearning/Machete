
# sql

This directory contains some (usually very old) scripts that have had some role in the development or management of
Machete data over the life of the project. Included in these scripts are fossils that probably predate Entity Framework,
as well as many other scripts that are still useful today. _All_ of these scripts, as far as we can tell, have at least
_some_ historical value. Please think twice before deleting them.

<hr />

### `ef`
Scripts for managing Entity Framework; notably, dealing with the `__EFMigrationsHistory` table that is occasionally the
bane of our existence.

<hr />

### `etl`

Scripts for ETL processes or manipulating very large amounts of data in some way. It's notable that much of the initial
data that was put into Machete from the various centers came from Excel spreadsheets, Microsoft Access databases,
Google Docs, and other various systems in use by the centers at different points in time. This folder contains examples
of how we've tackled those processes in the past.

<hr />

### `prehistory`

Pretty much just what it sounds like. Useful scripts for seeing what things were like before there was a before.

<hr />

### `recovery`

Various recovery scripts. Notably, a restoration script from when all of the centers were running on a single SQL Server
instance, and two scripts that show how to recover a lost employer.

Sometimes centers delete employers.  

We can restore those employers but it's kind of a painful process. Here's what you do:  

1. Restore an earlier copy of the database in Azure.
1. Connect to the restored database and run the sp_rename.sql script. Replace `id` and `EmployerID` with the value of
 the deleted employer. You can find the deletion action with the record ID in NLog. The `select` statements will give
  you the employer record, their work orders, and their work assignments as separate results.
1. Connect to the live database and run the restore_employer.sql script. You will need to provide the proper
 credentials in place of the empty strings. Also, you'll need to put the name of the restored database and the server
  name in place of the dummy values.

That's it! There's a little test `select` statement at the end of the restore_employer.sql script so that you can see
 the result of the `insert`. The rows returned should match the number of rows for work assignments in the final
  `select` statement from sp_rename.sql.


<hr />

### `reports`

It could be reasonably said that the whole purpose of the Machete program is to enable the centers to create reports,
mostly for fundraising purposes. A considerable archive of attempts to facilitate that process exists in this folder.
Enter at your own risk.


<hr />

Here is a cup of coffee. ☕️
