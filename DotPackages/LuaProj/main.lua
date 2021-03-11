local Object = require("DotLua/OOP/Object")
local Delegate = require("DotLua/OOP/Delegate")

local function TestObject(obj)
    print("Object.GetClassName = "..obj:GetClassName())
    print('Object.GetBaseClass = ' .. tostring(obj:GetBaseClass()))
    print('Object.GetType = ' .. obj:GetType())
    print('Object.IsClass = ' .. tostring(obj:IsClass()))
    print('Object.IsEnum = ' .. tostring(obj:IsEnum()))
    print('Object.IsDelegate = ' .. tostring(obj:IsDelegate()))
    print('Object.IsEvent = ' .. tostring(obj:IsEvent()))
    print('Object.ToString = ' .. tostring(obj:ToString()))
end


local function TestDelegate()
    print(Delegate:GetClassName())
end



local function main()
    local obj = Object()
    TestObject(obj)

end

main()
