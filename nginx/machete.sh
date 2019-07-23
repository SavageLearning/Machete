#!/bin/bash

'cp' ../certs/appsettings.json .
nginx >> ../certs/nginx.log 2>&1 &
dotnet Machete.Web.dll >> ../certs/machete.log 2>&1
