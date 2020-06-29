@ECHO OFF

SET BSOAGenerator="%~dp0..\bld\bin\Release\BSOA.Generator\netcoreapp3.1\BSOA.Generator.exe"
PUSHD %~dp0

ECHO Generating Unit Test models...
%BSOAGenerator% "Schemas\Person.UnitTest.V1.schema.json" "..\BSOA.Test\Model\V1"
%BSOAGenerator% "Schemas\Person.UnitTest.V2.schema.json" "..\BSOA.Test\Model\V2"

POPD