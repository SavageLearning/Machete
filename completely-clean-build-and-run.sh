# https://github.com/Microsoft/msbuild/issues/3362
export MSBUILDDISABLENODEREUSE=1

cd ../..

dotnet clean
cd UI
npm run build-local-dev
npm run start-local-dev &
cd ..
dotnet build
dotnet run --project Machete.Web --configuration=Debug

cd Machete.Util/sh
