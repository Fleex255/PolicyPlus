# Compiling Policy Plus
Policy Plus is developed with [Visual Studio 2017 Community](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx). (Fancier Visual Studio editions will work too.)

To compile, open `PolicyPlus.sln` or `PolicyPlus.vbproj` (from the `PolicyPlus` folder) in Visual Studio. Set the *Solution Configurations* dropdown to Release (or Debug for a debug build).
Choose *Build* | *Build PolicyPlus* from the main menu. The result is `PolicyPlus.exe`, which is set to run on 32-bit or 64-bit environments, preferring to run in a 64-bit process.
You can find that file in the subfolder of `bin` that corresponds to the build type.