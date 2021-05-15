#!/bin/bash
set -ex

cd Machete.Web && dotnet publish -o published && cd ..
if [ $APPVEYOR_REPO_BRANCH == 'master' ]; then docker build -t ndlonmachete/debian:$APPVEYOR_BUILD_VERSION .; fi
if [ $APPVEYOR_REPO_BRANCH != 'master' ]; then docker build -t ndlonmachete/debian:$APPVEYOR_BUILD_VERSION-beta .; fi
docker images
docker login -u chaim1221 -p "$DOCKER_PASSWORD"
if [ $APPVEYOR_REPO_BRANCH == 'master' ]; then docker push ndlonmachete/debian:$APPVEYOR_BUILD_VERSION; fi
if [ $APPVEYOR_REPO_BRANCH != 'master' ]; then docker push ndlonmachete/debian:$APPVEYOR_BUILD_VERSION-beta; fi
if [ $APPVEYOR_REPO_BRANCH == 'master' ]; then cd github && npm install && node github.js create-machete-release && cd ..; fi  
