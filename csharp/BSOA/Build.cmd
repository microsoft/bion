@ECHO OFF
SET NuGetLocal=%SystemDrive%\NuGetLocal
IF NOT "%1"=="" ( SET NuGetLocal=%1 )

PUSHD "%~dp0"

ECHO.
ECHO Build BSOA
ECHO ==========
dotnet build BSOA.sln -c Release

ECHO.
ECHO Test BSOA
ECHO =========
dotnet test BSOA.sln

ECHO.
ECHO Build NuGet Packages
ECHO ====================
dotnet pack BSOA.sln -c Release -o "bin\NuGet" --include-symbols --include-source --no-build

ECHO.
ECHO Clearing BSOA from Local NuGet Cache
ECHO ====================================
FOR /F "delims=" %%C IN ('dir /b "%USERPROFILE%\.nuget\packages\bsoa*"') DO (
    ECHO - "%USERPROFILE%\.nuget\packages\%%C
    RMDIR "%USERPROFILE%\.nuget\packages\%%C" /S /Q
)

IF EXIST "%NuGetLocal%" (
  ECHO.
  ECHO Copying to NuGet Local [%NuGetLocal%]
  ECHO =====================================
  XCOPY /Y "bin\NuGet\*.*" "%NuGetLocal%\"
)

POPD



