#!/bin/bash

# https://github.com/Microsoft/msbuild/issues/3362

trap 'kill $(jobs -p)' EXIT

echo "This script runs Machete in production mode, and should not be run simultaneously with a development build."
sleep 2

export MSBUILDDISABLENODEREUSE=1

if [ ! -f machete1env.json ]; then ./make_env_file.sh ; fi

for var in $(cat machete1env.list); do
  export $var
done

dotnet clean

cd UI
if [ $(ls | wc -l) -eq 0 ]; then cd .. ; git submodule update --init --force ; cd UI ; fi
npm install
npm run build-local-dev
npm run start-local-dev &
cd ..

dotnet build
dotnet run --project Machete.Web --configuration=Release
