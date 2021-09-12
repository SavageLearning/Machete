
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

LABEL maintainer="chaimeliyah@gmail.com"
ENV MACHETE_USE_HTTPS_SCHEME=https

# create and set working directory
RUN mkdir -p /app/api/Content /app/api/Identity /app/api/dist 

COPY ./Machete.Web/published/ /app/api
COPY ./Machete.Web/Content /app/api/Content
COPY ./Machete.Web/Identity /app/api/Identity
COPY ./UI/dist /app/api/dist
COPY ./appsettings.json /app/api
WORKDIR /app/api

ENTRYPOINT ["dotnet", "Machete.Web.dll"]
