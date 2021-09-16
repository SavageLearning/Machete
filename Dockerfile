
# First stage of multi-stage build
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /admin
# copy the contents of agent working directory on host to workdir in container
COPY . ./

# dotnet commands to build, test, and publish
RUN dotnet publish -o output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

LABEL maintainer="chaimeliyah@gmail.com"
ENV MACHETE_USE_HTTPS_SCHEME=https

# create and set working directory
RUN mkdir -p /app/api/Content /app/api/Identity /app/api/dist 
COPY --from=build-env /admin/output/ /app/api
COPY ./Machete.Web/Content /app/api/Content
COPY ./Machete.Web/Identity /app/api/Identity
COPY ./UI/dist /app/api/dist
WORKDIR /app/api
ENTRYPOINT ["dotnet", "Machete.Web.dll"]
