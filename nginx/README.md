#### `nginx`  

This directory contains configuration files for the Machete nginx container defined in
`../Dockerfile`. They are copied to the container by that file. A brief description follows.  

`nginx.conf`  
This is the root configuration file for the instance of `nginx` running in the Machete
container. It sets some global rules, but leaves site-specific configuration to the files
that it includes with this line:
```
    include /etc/nginx/conf.d/*.conf;
```  
  
`conf.d/`  
Named after the `/etc/nginx/conf.d/` directory in the Machete nginx container, this directory
contains example setup files, each of which contain a single server directive. Each directive
represents a worker center. The actual configuration used in production is kept separate.

`machete.sh`
This shell file does two simple things; it starts `nginx`, and also runs the `dotnet
Machete.Web.dll` command. It exists because there can only be one `CMD` Dockerfile directive
for a given Docker container.
