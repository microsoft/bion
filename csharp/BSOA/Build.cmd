@ECHO OFF
SET NuGetLocal=%SystemDrive%\NuGetLocal
IF NOT "%1"=="" ( SET NuGetLocal=%1 )

PUSHD "%~dp0"

ECHO.
ECHO Build and Pack BSOA
ECHO ===================
dotnet pack BSOA.sln -c Release -o "bld\NuGet" --include-source

ECHO.
ECHO Test BSOA
ECHO =========
dotnet test BSOA.sln -c Release

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
  XCOPY /Y "bld\NuGet\*.*" "%NuGetLocal%\"
)

POPD



