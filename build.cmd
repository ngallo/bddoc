ECHO OFF

SET CheckoutDir=%~dp0
SET CheckoutDir=%CheckoutDir:~0,-1%
SET NuGetEXE=C:\temp\tools\NuGet.CommandLine.2.8.3\tools\NuGet.exe
SET SolutionPlatform=Any CPU
SET SolutionConfiguration=Debug

if EXIST %SystemCheckoutDir%\Microsoft.NET\Framework64 (
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
