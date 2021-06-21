@ECHO OFF

output\DotTool.Config.exe -f ..\Test.xlsx -o D:\config-output -t Lua -p All -v true -l true -m lua-opt-template.txt

PAUSE