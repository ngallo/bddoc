ECHO OFF
SET CheckoutDir=%~dp0
SET CheckoutDir=%CheckoutDir:~0,-1%
SET SourceDir=%CheckoutDir%\samples
SET NuGetEXE=C:\temp\tools\NuGet.CommandLine.2.8.3\tools\NuGet.exe
SET SolutionRelativePath=\BDDoc.Samples.sln
SET SolutionConfiguration=Release
SET SolutionPlatform=Any CPU

if EXIST %SystemCheckoutDir%\Microsoft.NET\Framework64 (
	set msbuild=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe
)else (
	set msbuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe
)

set bddocbuild=%msbuild% /nologo msbuild\bddoc-samples.targets
if "%1" EQU "" (
	%bddocbuild% /t:Compile
)else (
	%bddocbuild% /t:%*
)
