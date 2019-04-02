
FROM nginx:1.15.9

LABEL maintainer="chaimeliyah@gmail.com"

# TODO: undo base container's EXPOSE 80
EXPOSE 443

# ENV commands
ENV DEBIAN_FRONTEND=noninteractive
ENV MACHETE_USE_HTTPS_SCHEME=https

# install dotnet core 
RUN apt-get update && \
    apt-get install --no-install-recommends -yq wget gnupg apt-transport-https ca-certificates && \
    wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg && \
    mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ && \
    wget -q https://packages.microsoft.com/config/debian/9/prod.list && \
    mv prod.list /etc/apt/sources.list.d/microsoft-prod.list && \
    chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg && \
    chown root:root /etc/apt/sources.list.d/microsoft-prod.list && \
    apt-get update && \
    apt-get install -yq aspnetcore-runtime-2.1 && \
    apt-get remove -y wget && \
    apt-get clean -y && \
    apt-get autoremove -y && \
    rm -rf /var/lib/apt/lists/*

# dev tools
RUN apt-get update && \
    apt-get install --no-install-recommends -yq lsof vim iputils-ping && \
    apt-get clean -y && \
    apt-get autoremove -y && \
    rm -rf /var/lib/apt/lists/*

# create and set working directory
RUN mkdir -p /app/api/Content && \
    mkdir /app/api/Identity && \
    mkdir /app/api/dist && \
    rm /etc/nginx/conf.d/default.conf

COPY ./Machete.Web/published/ /app/api
COPY ./Machete.Web/Content /app/api/Content
COPY ./Machete.Web/Identity /app/api/Identity
COPY ./UI/dist /app/api/dist
COPY ./nginx/machete.sh /app/api
COPY ./nginx/nginx.conf /etc/nginx/nginx.conf
COPY ./nginx/conf.d/ /etc/nginx/conf.d/

# TODO this shouldn't be in the Dockerfile
COPY ./UI/ssl /app/certs

WORKDIR /app/api

# start app
# CMD ["dotnet", "/app/api/Machete.Web.dll"]
CMD "/bin/bash"
