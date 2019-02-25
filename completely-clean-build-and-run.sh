# https://github.com/Microsoft/msbuild/issues/3362
export MSBUILDDISABLENODEREUSE=1

dotnet clean
cd UI
npm run build-local-dev
cd ..
dotnet build
dotnet run --project Machete.Web --configuration=Debug
