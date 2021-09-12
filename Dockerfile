
FROM nginx:1.19.10

LABEL maintainer="chaimeliyah@gmail.com"
ENV MACHETE_USE_HTTPS_SCHEME=https

# install dotnet core 
RUN apt-get update && \
    apt-get install --no-install-recommends -yq wget gnupg apt-transport-https ca-certificates lsof vim iputils-ping && \
    wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg && \
    mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ && \
    wget -q https://packages.microsoft.com/config/debian/9/prod.list && \
    mv prod.list /etc/apt/sources.list.d/microsoft-prod.list && \
    chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg && \
    chown root:root /etc/apt/sources.list.d/microsoft-prod.list && \
    apt-get update && \
    apt-get install -yq aspnetcore-runtime-3.1 && \
    apt-get clean -y && \
    apt-get autoremove -y && \
    rm -rf /var/lib/apt/lists/*

# create and set working directory
RUN mkdir -p /app/api/Content /app/api/Identity /app/api/dist 

COPY ./Machete.Web/published/ /app/api
COPY ./Machete.Web/Content /app/api/Content
COPY ./Machete.Web/Identity /app/api/Identity
COPY ./UI/dist /app/api/dist
COPY ./appsettings.json /app/api
WORKDIR /app/api

ENTRYPOINT ["dotnet", "Machete.Web.dll"]
