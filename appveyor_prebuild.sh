#!/bin/bash
set -ex

node --version
dotnet --version
pwd
cd UI
npm install
npm run --silent build-prod
ls -l .
ls -l ./dist/
cd ..
#sed -i "s/APPVEYOR_VERSION/$APPVEYOR_BUILD_VERSION/" "./Machete.Web/Views/Home/Index.cshtml"
  