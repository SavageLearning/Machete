
# Machete

[![Build and test dotnet core](https://github.com/SavageLearning/Machete/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/SavageLearning/Machete/actions/workflows/dotnet.yml)
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

`☕️ 💻 🐈`

### Setup scripts
<hr>

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
2. Prompt you to create the environment variables file if it doesn't exist.
3. Source the environment variables file.
4. Clean up any existing build(s).
5. Initialize the submodule, if it isn't initialized.
6. Start the Angular webpack server as a background process.
7. Build and run Machete.

`🎺 💃 🇲🇽`

### How to connect to the database
<hr>
Download DataGrip. It has a 30 day evaluation period, but is not free software.

*`⌘-;`* will configure a data source, if you have one defined; *`⌘-↵`* is execute query, NOT F5
if you don't you'll need the drivers for your data source. it will prompt you in the config window. right click on the area to the left to add a data source.

1.14:
if prod: Create SSH tunnel
Host: `localhost`  Port: `1433` (or whatever you configure in the tunnel)
User: `ask admins`
Password: `ask admins`

Test Connection + Select Schemas from the middle tab

Select two databases using *`⌘`* to select multiple, then *`⌘-D`* will schema compare.


## People who use this free software include:
<hr>

[![Casa Latina](./Machete.Util/misc/casa-latina.png)](https://casa-latina.org/)
[![Portland Voz](./Machete.Util/misc/voz.png)](https://portlandvoz.org/)
[![NDLON](https://ndlon.org/wp-content/uploads/2017/12/NDLON-Logo-Wide-Red-60.png)](https://ndlon.org/)

#### ...and many more!
