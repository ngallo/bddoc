ECHO OFF

SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe
IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto build
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:build
SET CheckoutDir=%~dp0
SET CheckoutDir=%CheckoutDir:~0,-1%
SET NuGetEXE=%CheckoutDir%\.nuget\nuget.exe
SET SolutionPlatform=Any CPU
SET SolutionConfiguration=Debug

if EXIST %SystemRoot%\Microsoft.NET\Framework64 (
    SET msbuild=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe
)else (
    SET msbuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
)

SET _build=
SET _buildarguments=
SET SourceDir=
SET SolutionRelativePath=

if "%1" EQU "-samples" (
    SET SourceDir=%CheckoutDir%\samples
    SET SolutionRelativePath=\BDDoc.Samples.sln
    SET _build=%msbuild% /nologo msbuild\bddoc-samples.targets
	if "%2" EQU "-r" (
		SET SolutionConfiguration=Release
		for /f "tokens=1-2*" %%a in ("%*") do set _buildarguments=%%c
	) else (
		for /f "tokens=1-2*" %%a in ("%*") do set _buildarguments=%%b
	)	
)else (
    SET SourceDir=%CheckoutDir%\src
    SET SolutionRelativePath=\BDDoc.sln
    SET _build=%msbuild% /nologo msbuild\bddoc.targets
	if "%1" EQU "-r" (
		SET SolutionConfiguration=Release
		for /f "tokens=1-1*" %%a in ("%*") do set _buildarguments=%%b
	) else (
		for /f "tokens=1-1*" %%a in ("%*") do set _buildarguments=%%a
	)	
)

if "%_buildarguments%" EQU "" (
	SET _buildarguments=Compile
)

%_build% /t:%_buildarguments%

if "%1" EQU "-samples" (
    if exist %CheckoutDir%\output-samples\Documentation\HTML\index.html start %CheckoutDir%\output-samples\Documentation\HTML\index.html
)
