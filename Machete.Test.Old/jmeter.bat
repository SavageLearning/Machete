@set JM_START=start
@set JM_LAUNCH=javaw.exe
@call %ChocolateyInstall%\lib\jmeter\tools\apache-jmeter-2.13\bin\jmeter.bat -Jincludecontroller.prefix='C:\Users\Administrator\Documents\Machete\Machete.Test' %ARGS%
@set JM_START=
@set JM_LAUNCH=