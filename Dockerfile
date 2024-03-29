FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-dotnet
ARG machete_version=UNSET
ENV machete_version=$machete_version
WORKDIR /admin
COPY . ./
RUN dotnet tool restore
RUN dotnet build --no-incremental
RUN sed -i "s/APPVEYOR_VERSION/$machete_version/" "./Machete.Web/Views/Home/Index.cshtml"
RUN dotnet publish -o output Machete.Web
FROM node:12.22.6-alpine3.13 as build-nodejs
WORKDIR /app
COPY ./UI ./
RUN npm install
RUN npm run --silent build-prod


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

LABEL maintainer="chaimeliyah@gmail.com"
ENV MACHETE_USE_HTTPS_SCHEME=https

# create and set working directory
RUN mkdir -p /app/api/Content /app/api/Identity /app/api/dist
COPY --from=build-dotnet /admin/output/ /app/api
COPY --from=build-nodejs /app/dist /app/api/dist
WORKDIR /app/api
ENTRYPOINT ["dotnet", "Machete.Web.dll"]
