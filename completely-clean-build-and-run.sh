# https://github.com/Microsoft/msbuild/issues/3362
export MSBUILDDISABLENODEREUSE=1

if [ ! -f machete1env.list ]; then ./make_env_file.sh ; fi

for var in $(cat machete1env.list); do
  export $var
done

dotnet clean

cd UI
if [ $(ls | wc -l) -eq 0 ]; then cd .. ; git submodule update --init --force ; cd UI ; fi
npm install
npm run build-local-dev
npm run start-local-dev &
cd ..

dotnet build
dotnet run --project Machete.Web --configuration=Debug
