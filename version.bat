@echo off
set VF=PolicyPlus\Version.vb
echo ' DO NOT MODIFY THIS FILE. To update it, run version.bat again. > %VF%
echo Module VersionHolder >> %VF%
echo     Public Const Version As String = ^" >> %VF%
git describe --always >> %VF%
echo ^" >> %VF%
echo End Module >> %VF%