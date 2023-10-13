@echo off

for /f "tokens=* usebackq" %%a in (`git describe --always`) do (
    set VERSION=%%a
    )
)

echo %VERSION%
set VF=PolicyPlus.csharp\VersionHolder.cs
echo // DO NOT MODIFY THIS FILE. It will be overwritten by version.bat.> %VF%
echo namespace PolicyPlus.csharp>> %VF%
echo {>> %VF%
echo     internal static class VersionHolder>> %VF%
echo     {>> %VF%
echo         public const string Version = ^"%VERSION%^";>> %VF%
echo     }>> %VF%
echo }>> %VF%
