# https://github.com/Microsoft/msbuild/issues/3362
export MSBUILDDISABLENODEREUSE=1

dotnet clean

# assumes you've run git submodule update --init #--force #etc.
cd UI
npm run build-local-dev
npm run start-local-dev &
cd ..

dotnet build
dotnet run --project Machete.Web --configuration=Debug
