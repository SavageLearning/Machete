#!/bin/bash

# This file performs some steps for your local system that would already be done on a machine running with Chef.
# It will get you running SQL Server in a container on a fresh system.

docker network create --driver bridge machete-bridge
docker network inspect machete-bridge

source ./new-db-use-with-caution.sh

# This would be nice, but you're going to run into the problem that you don't have any secrets provisioned.
# Which is fine, except that it disables Google and Facebook login. So I'm leaving it commented out for now.
# ./dockerize.sh && run-machete1.sh
