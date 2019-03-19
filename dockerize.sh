#!/bin/bash

set -e #-x

export MSBUILDDISABLENODEREUSE=1

docker stop machete1
docker rm machete1

dotnet clean

cd UI
if [[ -d dist ]]; then
  rm -rf dist
fi

# TODO build something more prod-like?
npm run build-local-dev
cd ..

# dotnet build
cd Machete.Web
if [[ -d published ]]; then
  rm -rf published
fi
dotnet publish -o published
cd ..

# TODO fix versioning
docker build -t machete/debian:1.15.0 .

echo "to run, type:"
echo ""
# please don't remove these lines, they have historical value; if you feel that's no longer the case contact the author.
# echo "docker run --rm -it --network host -e ASPNETCORE_URLS=\"https://+\" -e ASPNETCORE_HTTPS_PORT=4213 -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v ${HOME}/.aspnet/https:/https/ --name machete1 machetecontainer"
# echo "docker run -dit --name machete1 --network machete-bridge -p 443:443 machete/debian:1.15.0"
echo "./run-machete1.sh"
