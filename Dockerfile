
FROM ndlonmachete/debian-base:0.1.13

LABEL maintainer="chaimeliyah@gmail.com"
ENV MACHETE_USE_HTTPS_SCHEME=https

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
