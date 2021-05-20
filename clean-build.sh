#!/bin/bash
set -e

# https://github.com/Microsoft/msbuild/issues/3362
export MSBUILDDISABLENODEREUSE=1
# https://stackoverflow.com/questions/60255563/how-to-resolve-error-rzc-discover-exited-with-code-150
export DOTNET_ROLL_FORWARD_ON_NO_CANDIDATE_FX=2

build_ui() {
  cd UI
  if [ $(ls | wc -l) -eq 0 ]; then cd .. ; git submodule update --init --force ; cd UI ; fi
  if [[ -d dist ]]; then rm -rf dist; fi
  npm install
  npm run build-local-dev
  cd ..
}

build_web() {
  dotnet clean
  dotnet build
}

make_docker_container() {
  current_version=$(curl -s https://api.github.com/repos/SavageLearning/machete/releases/latest | jq .tag_name | sed s/\"//g)
  IFS='.' read -r -a version_array <<< ${current_version}
  next_version=${version_array[0]}.${version_array[1]}.$((version_array[2] + 1))

  docker stop machete1 || true
  docker rm machete1 || true

  cd Machete.Web
  if [[ -d published ]]; then
    rm -rf published
  fi
  dotnet publish -o published
  cd ..

  docker build -t ndlonmachete/debian:${next_version}-dev .
  ./Machete.Util/sh/run-machete1.sh
}

make_env_file() {
  MACHETE_Authentication__State=fakestate
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
}

run_local() {
  cd UI
  npm run start-local-dev &
  cd ..
  dotnet run --project Machete.Web --configuration=Debug
}

if [ ! -f machete1env.list ]; then make_env_file ; fi
for var in $(cat machete1env.list); do export $var; done

printf "Performing clean build of UI and Web...\n"
build_ui
build_web
printf "...done!\n"

case "$1" in
"docker")
  make_docker_container
  ;;
"run")
  run_local
  ;;
*)
  printf "No target for build selected; you may now run manually, or re-run with either 'run' or 'docker' as first parameter.\n"
  ;;
esac
