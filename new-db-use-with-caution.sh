set +H

if [ "${BASH_SOURCE[0]}" -ef "$0" ]
then
    echo "Hey, you should source this script, not execute it!"
    exit 1
fi

sudo docker rm -f $MACHETE_SQL_DOCKER_CONTAINER
sudo docker pull mcr.microsoft.com/mssql/server
export MACHETE_SQL_DOCKER_CONTAINER=$(sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=passw0rD!' -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2017-latest)
# sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=passw0rD!' -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2017-latest >> current_db_container_hash
# cat current_db_container_hash
# export MACHETE_SQL_DOCKER_CONTAINER=$(cat current_db_container_hash)

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

# docker exec -it sql1 bash
