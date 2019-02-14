cd UI
npm run build-mvc-embedded
cd ..
dotnet build
dotnet run --project=Machete.Web --configuration=Debug
