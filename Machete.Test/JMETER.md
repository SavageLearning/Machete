HOWTO
======

 * Install jmeter with chocolatey
 * change directories to \Machete.Test\
 * execute the following:

```
 @set JM_START=start
 @set JM_LAUNCH=javaw.exe
 @call %ChocolateyInstall%\lib\jmeter\tools\apache-jmeter-2.13\bin\jmeter.bat  -DLOGIN_ID=<valid id> -DLOGIN_PASSWORD=<valid pw>
```
