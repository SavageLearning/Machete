#!/bin/bash

echo '' > machete1env.json

echo "{" >> machete1env.json
#echo "  \"Authentication\": {" >> machete1env.json
for i in $(cat machete1env.list); do
  fieldname=$(echo $i | awk -F= '{print $1}' | sed s/__/:/g | awk -F_ '{print $2}')
  fieldvalue=$(echo $i | awk -F= '{print $2}')
  echo "    \"${fieldname}\": \"${fieldvalue}\"," >> machete1env.json
done

echo "    \"stupidHack\": \"I don't have a comma\"" >> machete1env.json

#echo "  }" >> machete1env.json
echo "}" >> machete1env.json
