#!/bin/bash
set -ex

node --version
dotnet --version
pwd
#git submodule init
#git submodule update
cd UI
npm install
npm install @angular/cli
npm run --silent build-prod
ls -l .
ls -l ./dist/
cd ..
#sed -i "s/APPVEYOR_VERSION/$APPVEYOR_BUILD_VERSION/" "./Machete.Web/Views/Home/Index.cshtml"
  