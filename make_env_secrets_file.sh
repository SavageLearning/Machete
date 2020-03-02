#!/bin/bash

export MACHETE_Authentication__State=fakestate
read -p "Google Client ID: " MACHETE_Authentication__Google__ClientId
read -s -p "Google Client Secret: " MACHETE_Authentication__Google__ClientSecret
echo ""
read -p "Facebook App ID: " MACHETE_Authentication__Facebook__AppId
read -s -p "Facebook App Secret: " MACHETE_Authentication__Facebook__AppSecret
echo ""

if [ -f machete1env.list ]; then rm machete1env.list; fi

echo "MACHETE_Authentication__State=$MACHETE_Authentication__State" >> machete1env.list
echo "MACHETE_Authentication__Google__ClientId=$MACHETE_Authentication__Google__ClientId" >> machete1env.list
echo "MACHETE_Authentication__Google__ClientSecret=$MACHETE_Authentication__Google__ClientSecret" >> machete1env.list
echo "MACHETE_Authentication__Facebook__AppId=$MACHETE_Authentication__Facebook__AppId" >> machete1env.list
echo "MACHETE_Authentication__Facebook__AppSecret=$MACHETE_Authentication__Facebook__AppSecret" >> machete1env.list

echo "You must now:"
echo "source machete1env.list"
echo ""
