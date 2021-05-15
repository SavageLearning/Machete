#!/bin/bash
set -ex

cd Machete.Web && dotnet publish -o published && cd ..
if [ $APPVEYOR_REPO_BRANCH == 'master' ]; then 
  docker build -t ndlonmachete/debian:$APPVEYOR_BUILD_VERSION .
  docker images
  docker login -u chaim1221 -p "$DOCKER_PASSWORD"
  docker push ndlonmachete/debian:$APPVEYOR_BUILD_VERSION
  cd github && npm install && node github.js create-machete-release && cd ..
fi
if [ $APPVEYOR_REPO_BRANCH != 'master' ]; then
  docker build -t ndlonmachete/debian:$APPVEYOR_BUILD_VERSION-beta .
  docker images
  docker login -u chaim1221 -p "$DOCKER_PASSWORD"
  docker push ndlonmachete/debian:$APPVEYOR_BUILD_VERSION-beta
fi