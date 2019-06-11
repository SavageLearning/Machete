#!/bin/bash

'cp' ../certs/appsettings.json .
nginx &
dotnet Machete.Web.dll
