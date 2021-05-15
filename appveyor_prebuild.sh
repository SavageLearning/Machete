#!/bin/bash
set -ex

node --version
dotnet --version
dotnet clean
cd UI
npm install
npm run --silent build-prod
npm run build-prod 
cd ..
sed -i "s/APPVEYOR_VERSION/$APPVEYOR_BUILD_VERSION/" "./Machete.Web/Views/Home/Index.cshtml"
  