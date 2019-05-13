
# Machete

[![Build status](https://ci.appveyor.com/api/projects/status/4xhan2xt89f7sgji/branch/feature/multi-database?svg=true)](https://ci.appveyor.com/project/savagelearning/machete-k8wo2/branch/master)
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
please contact chaim@ndlon.org.

`☮️ 💟 🍁`

## How to get started
<hr>

To get started developing with Machete, you will need a few things. Currently we are pretty much all developing on Mac
OS X, but as this is a `dotnet core` project, you can pretty much develop on any platform. First, go ahead and make a
fork of this repo; we only accept internal pull requests from our contributors, but you can submit a request from your
own fork any time you like and we will review it. You can clone your repo using your own credentials.

We recommend using [Visual Studio Code](https://code.visualstudio.com/download) as your primary editing tool. It is easy
to learn and to configure, and it plays incredibly well with `dotnet core` and other cross-platform applications.

You will also need [Docker](https://www.docker.com/get-started). Keep in mind that different platforms have different
levels of support for the Docker engine. Depending on your platform, Docker may be running inside a VM with very little
filesystem access. You probably won't need to worry about this unless you become more involved with the project.

Finally, you will need the [`dotnet` CLI](https://dotnet.microsoft.com/download).

Great, now you should be good to go!

`☕️ 💻 🐈`

## Setup scripts
<hr>

There are a few setup scripts that can help get you up and running. These scripts are _experimental_ and any problems or
inquiries should be directed to the [email](chaim@ndlon.org) above.

`./Machete.Util/sh/initial-setup.sh`  
This will create a Docker network for you, `machete-bridge`, and attempt to create a database using the next script.

`./Machete.Util/sh/new-db-use-with-caution.sh`  
Aptly named, this file will dispose of whatever database container you have running (as long as you have the environment
variable for it set, which the script does), and create an entirely new database container.

`./Machete.Util/sh/run-machete1.sh`
This script runs the Machete.Web.dll binary inside a container behind an NginX proxy. Not really needed for development.

`🎺 💃 🇲🇽`

## People who use this free software include:
<hr>

[![Casa Latina](./Machete.Util/misc/casa-latina.png)](https://casa-latina.org/)
[![Portland Voz](./Machete.Util/misc/voz.png)](https://portlandvoz.org/)
[![NDLON](https://ndlon.org/wp-content/uploads/2017/12/NDLON-Logo-Wide-Red-60.png)](https://ndlon.org/)

#### ...and many more!
