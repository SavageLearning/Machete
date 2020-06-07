

# Machete 

[![Build status](https://ci.appveyor.com/api/projects/status/4xhan2xt89f7sgji/branch/master?svg=true)](https://ci.appveyor.com/project/savagelearning/machete-k8wo2/branch/master)
[![Join the chat at https://gitter.im/machete-project/Lobby](https://badges.gitter.im/machete-project/Lobby.svg)](https://gitter.im/machete-project/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![GPLv3 license](https://img.shields.io/badge/License-GPLv3-blue.svg)](http://www.gnu.org/licenses/gpl-3.0.en.html)

Machete is a web application that manages a day labor referral service. Machete tracks work orders for employers looking
for short term, casual labor. It also provides a means for matching laborers with specific skills to requests made by 
employers for skilled labor.

Machete performs the following tasks:

1. Stores basic contact information for all persons associated with the Machete system
2. Stores biographical and membership information on its members
3. Records member sign-in using a identification number through either a bar code scanner or the keyboard
4. Captures employer, work order, and work assignment details into a relational database
5. Provides a fast, flexible method of dispatching workers to work orders based on English level, skill level, and 
employer requests.

[![Machete Workflow](https://raw.githubusercontent.com/wiki/savagelearning/machete/images/8-point-workflow.png)](https://github.com/SavageLearning/Machete/wiki)

This README is mostly for software developers. If you would like to know more about Machete or are interested in a demo,
see our contact information below.

<hr />
<br />

## How to get started ☕️

To get started developing with Machete, you will need a few things. Currently we are pretty much all developing on Mac
OS X, but as this is a `dotnet core` project, you can pretty much develop on any platform. First, go ahead and make a
fork of this repo; we only accept internal pull requests from our contributors, but you can submit a request from your
own fork any time you like and we will review it. You can clone your repo using your own credentials.

We recommend using [Visual Studio Code](https://code.visualstudio.com/download) as your primary editing tool. It is easy
to learn and to configure, and it plays incredibly well with `dotnet core` and other cross-platform applications.

You will also need [Docker](https://www.docker.com/get-started). Keep in mind that different platforms have different
levels of support for the Docker engine. Depending on your platform, Docker may be running inside a VM with very little
filesystem access (looking at you, Mac). You probably won't need to worry about this unless you become more involved with 
the project. If you need help, seek help!

Additionally, you will need the [`dotnet` CLI](https://dotnet.microsoft.com/download).

Finally, you will need `npm` for your distribution. We recommend [downloading and installing NodeJS](https://nodejs.org/en/)
for your distribution. Once you have `npm` installed you will need to run `npm install -g @angular/cli` in the terminal.

If you are on a Mac, you will need to run `xcode-select --install` if you haven't already, and accept the license for the
Darwin linker (this is for Node dependencies). You'll also need to `sudo chown -R $USER: /usr/local/lib/node_modules` 
(because Mac).

Great, now you should be good to go!


<hr />
<br />


## Setup scripts 💻

There are a few setup scripts that can help get you up and running. These scripts are _experimental_ and any problems or
inquiries should be directed to the [email](chaim@ndlon.org) above.

`./Machete.Util/sh/new-db-use-with-caution.sh`  
Aptly named, this file will dispose of whatever database container you have running (as long as you have the environment
variable for it set, which the script does), and create an entirely new database container.

`./make_env_file.sh`  
This file will create an environment file that you can source to set certain variables. Only needed for OAuth development.

`./completely-clean-build-and-run.sh`
This file will:
1. [Disable msbuild 're-use'](https://github.com/Microsoft/msbuild/issues/3362) in `dotnet`. 
2. Prompt you to create the environment variables file if it doesn't exist.w
3. Source the environment variables file.
4. Clean up any existing build(s).
5. Initialize the submodule, if it isn't initialized.
6. Start the Angular webpack server as a background process.
7. Build and run Machete.



<hr />
<br />

## How to Connect to the Database 🔋
Download DataGrip. It has a 30 day evaluation period, but is not free software.

*`⌘-;`* will configure a data source. If you're connecting to a production instance, you'll have to create an SSH tunnel.
That is beyond the scope of this README. 

You'll need the drivers for your data source. it will prompt you in the config window. right click on the 
area to the left to add a data source.


Host: `localhost`  Port: `1433` (or whatever you have configured, e.g., when creating the tunnel)  
User: `ask admins`  
Password: `ask admins`

Click on `Test Connection` and then `Select Schemas` from the middle tab. You can Select two or more databases using *`⌘`*;
*`⌘-D`* will compare the schemas of the selected databases.

**_Additional Notes_** 🎺  
*`⌘-↵`* is execute query in DataGrip, **not** `F5` as in Visual Studio.

<hr />
<br />

## Contact Information: Developers' Corner 👩🏾‍💻
_Developers may add current contact information here if they feel like doing so._  
<br />
<br />
Jimmy - Architect - https://github.com/jcii  
Chaim - SWE - https://github.com/chaim1221 - chaim@ndlon.org  
Esteban - SWE - https://github.com/esteban-gs
<hr />
<br />

## People who use this free software 💃🏽
_Don't see your organization here? Send us a 300 by 200 pixel logo!_
<br />
<br />

[![Casa Latina](./Machete.Util/misc/casa-latina.png)](https://casa-latina.org/)
[![Portland Voz](./Machete.Util/misc/voz.png)](https://portlandvoz.org/)
[![NDLON](https://ndlon.org/wp-content/uploads/2017/12/NDLON-Logo-Wide-Red-60.png)](https://ndlon.org/)

**...and many more!** 

# 🇲🇽 🇬🇹 🇧🇿 🇭🇳 🇸🇻 🇳🇮 🇨🇷 🇵🇷 🇨🇺 🇩🇴 🇭🇹 🇵🇦 🇻🇪 🇨🇴 🇪🇨 🇵🇪 🇧🇷 🇵🇾 🇺🇾 🇦🇷 🇨🇱
