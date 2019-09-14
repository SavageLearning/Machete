# Well aware this file is becoming kind of a nightmare. TODO cleanup.
set +H

if [ "${BASH_SOURCE[0]}" -ef "$0" ]
then
    echo "Hey, you should source this script, not execute it!"
    exit 1
fi

sudo docker rm -f $MACHETE_SQL_DOCKER_CONTAINER

if [ $? -ne 0 ]; then
  sudo docker rm -f sqlserver
fi

sudo docker pull mcr.microsoft.com/mssql/server
#sudo docker pull postgres

if [[ $(cat /etc/hosts | grep sqlserver | wc -l) -eq 0 ]]; then
#  sudo echo "127.0.0.1	sqlserver" >> /etc/hosts
   echo '127.0.0.1       sqlserver' | sudo tee -a /etc/hosts
fi

# this used to output the alread-running container name, it now does not :p so you'll have to delete manually if MACHETE_SQL_DOCKER_CONTAINER is not set.
# to make your life easier, just copy-paste the value it gives you between quotes and `export MACHETE_SQL_DOCKER_CONTAINER={that}`
export MACHETE_SQL_DOCKER_CONTAINER=$(sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=passw0rD!' --network machete-bridge -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server)
#export MACHETE_SQL_DOCKER_CONTAINER=$(sudo docker run -e 'POSTGRES_PASSWORD=passw0rD!' --network machete-bridge -p 5432:5432 --name postgresql -d postgres)
echo $MACHETE_SQL_DOCKER_CONTAINER

# sudo docker exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost \
#   -U SA -P 'passw0rD!' -Q $'CREATE USER dev WITH PASSWORD = \'passw0rD!\''
# sudo docker exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost \
#   -U SA -P 'passw0rD!' \
#   -Q $'EXEC sys.sp_addsrvrolemember @loginame = N\'dev\', @rolename = N\'sysadmin\';'
# sudo docker exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost \
#   -U SA -P 'passw0rD!' \
#   -Q 'ALTER SERVER ROLE [sysadmin] ADD MEMBER [dev]'

set -H
