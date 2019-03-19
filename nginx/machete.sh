#!/bin/bash

nginx &
dotnet Machete.Web.dll
