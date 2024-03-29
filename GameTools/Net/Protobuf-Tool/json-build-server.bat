@ECHO OFF

SET PBConfigToolPath=PBConfigTool\PBConfigTool.exe
SET ToolConfigPath=tool-config.xml
SET ProtoToolPath=protoc-3.13.0-win64\bin\protoc.exe
SET ProtoDir=configs\protos
SET OutputDir=output\server\json

IF NOT EXIST %OutputDir% (
    MD %OutputDir%
)

%PBConfigToolPath% -c %ToolConfigPath% -p Server -f Json -o %OutputDir%

pause