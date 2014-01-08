@echo off
rem %1 - project dir
rem %2 - output dir of dll
rem %3 - target name
rem %4 - configuration name

if (%4) == (Debug) goto :eof

goto :eof


echo Obfuscate assembly
"%ProgramFiles(x86)%\Eziriz\.NET Reactor\dotNET_Reactor.exe" -project "%1ForgeBotObfuscation.nrproj" -q
echo moving to obj\... since pubblishing will be taking it from here
rem move /y "%1%2%3_Secure\%3.exe" "%1obj\x86\Release"
