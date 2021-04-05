@ECHO OFF

SET Input_Proto_Path=protos
SET Output_PB_Path=output

SET Proto_EXE_PATH=protoc-3.13.0.exe

if not exist %Output_PB_Path% mkdir %Output_PB_Path%

FOR /r %%i in (%Input_Proto_Path%\*.proto) do (
    %Proto_EXE_PATH% --descriptor_set_out %Output_PB_Path%\%%~ni.pb %Input_Proto_Path%\%%~nxi
)

PAUSE