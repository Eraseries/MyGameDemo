@echo off&cd /d "%~dp0"
rem 将一个文件夹里的所有文件移动到另一个文件夹里
set #=Any questions&set _=WX&set $=Q&set/az=0x53b7e0b4
title %#% +%$%%$%/%_% %z%
set "oldfolder=F:\MyDemoGame\Android\asset\Package\0_1_0_0\Android"
set "newfolder=F:\MyDemoGame\Assets\StreamingAssets"
if not exist "%oldfolder%" (echo;"%oldfolder%" path error or not exist&pause&exit)
if "%oldfolder:~-1%" equ "\" set "oldfolder=%oldfolder:~,-1%"
if "%newfolder:~-1%" equ "\" set "newfolder=%newfolder:~,-1%"
robocopy "%oldfolder%" "%newfolder%" /move /e
echo;%#% +%$%%$%/%_% %z%
pause
exit