#!/bin/bash
set -ex

git submodule init
git submodule update
npm install @angular/cli
dotnet restore
#docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=passw0rD!' --network machete-bridge -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server
