@ECHO OFF

SET PBConfigToolPath=PBConfigTool\PBConfigTool.exe
SET ProtoConfigPath=configs\proto-config.xml
SET ProtoToolPath=protoc-3.11.4-win64\bin\protoc.exe
SET ProtoDir=configs\protos

SET IdTemplateFilePath=configs\script-template\csharp\pb-id-template.txt
SET ParserTemplateFilePath=configs\script-template\csharp\pb-parser-template.txt

SET OutputDir=output\csharp

FOR /R %%f IN (%ProtoDir%\*.proto) do (
    %ProtoToolPath% --csharp_out=%OutputDir% --proto_path=%ProtoDir% %%~nxf
)

%PBConfigToolPath% -c %ProtoConfigPath% -o %OutputDir% -t %IdTemplateFilePath% -p All -f Id
%PBConfigToolPath% -c %ProtoConfigPath% -o %OutputDir% -t %ParserTemplateFilePath% -p Client -f Parser
%PBConfigToolPath% -c %ProtoConfigPath% -o %OutputDir% -t %ParserTemplateFilePath% -p Server -f Parser