@ECHO OFF

SET BSOAGenerator="%~dp0bin\Release\netcoreapp3.1\BSOA.Generator.exe"
SET JSchemaToBsoaSchema="%~dp0..\JschemaToBsoaSchema\bin\Release\netcoreapp3.1\JSchemaToBsoaSchema.exe"
PUSHD %~dp0

ECHO Generating Tiny model...
%BSOAGenerator% "Schemas\TinyDemo.json" "..\RegionDemo\Model"

POPD