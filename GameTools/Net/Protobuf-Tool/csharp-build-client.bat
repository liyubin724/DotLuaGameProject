@ECHO OFF

SET PBConfigToolPath=PBConfigTool\PBConfigTool.exe
SET ToolConfigPath=tool-config.xml
SET ProtoToolPath=protoc-3.13.0-win64\bin\protoc.exe
SET ProtoDir=configs\protos
SET OutputDir=output\client\csharp

IF NOT EXIST %OutputDir% (
    MD %OutputDir%
)

FOR /R %%f IN (%ProtoDir%\*.proto) do (
    %ProtoToolPath% --csharp_out=%OutputDir% --proto_path=%ProtoDir% %%~nxf
)

%PBConfigToolPath% -c %ToolConfigPath% -p Client -f CSharp -o %OutputDir%

pause