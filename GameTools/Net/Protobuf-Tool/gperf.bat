@ECHO OFF
SET ProtoToolPath=protoc-3.11.4-win64\bin\protoc.exe
SET ProtoDir=GPerf
SET OutputDir=GPerf


FOR /R %%f IN (%ProtoDir%\*.proto) do (
    %ProtoToolPath% --csharp_out=%OutputDir% --proto_path=%ProtoDir% %%~nxf
)

pause