@ECHO OFF

PUSHD %~dp0bin\Release\netcoreapp3.1

ECHO Generating Unit Test models...
BSOA.Generator.exe "..\..\..\Schemas\Person.UnitTest.V1.schema.json" "..\..\..\..\BSOA.Test\Model\V1"
BSOA.Generator.exe "..\..\..\Schemas\Person.UnitTest.V2.schema.json" "..\..\..\..\BSOA.Test\Model\V2"

ECHO Generating Demo model...
BSOA.Generator.exe "..\..\..\Schemas\BsoaDemo.schema.json" "..\..\..\..\BSOA.Demo\Model" "Templates\Sarif\Team.cs"

POPD