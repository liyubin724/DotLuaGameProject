require('DotLua/Utils/stringEx')
require('DotLua/Utils/tableEx')
local Enum = require('DotLua/OOP/Enum')
local Delegate = require('DotLua/OOP/Delegate')
local Event = require('DotLua/OOP/Event')

local oop = {}

oop.isDebug = _G.isDebug or true

oop.assembly = {}
oop.using = function(path)
    local c = oop.assembly[path]
    if not c then
        c = require(path)

        oop.assembly[path] = c
    end
    return c
end

oop.ObjectType = oop.using('DotLua/OOP/ObjectType')
oop.Object = oop.using('DotLua/OOP/Object')

oop.class = require('DotLua/OOP/class')

oop.enum = function(name, values)
    return Enum(name, values)
end

oop.delegate = function(receiver, func)
    return Delegate(receiver, func)
end

oop.event = function()
    return Event()
end

oop.isclass = function(target)
    if not target then
        return false
    end

    local isClassFunc = target.IsClass
    if not isClassFunc then
        return false
    end

    return isClassFunc(target)
end

oop.isinstance = function(target)
    if not target then
        return false
    end

    local isInstanceFunc = target.IsInstance
    if not isInstanceFunc then
        return false
    end

    return isInstanceFunc(target)
end

oop.isclassorinstance = function(target)
    return oop.isclass(target) and oop.isinstance(target)
end

oop.isenum = function(target)
    if not target then
        return false
    end

    local isEnumFunc = target.IsEnum
    if not isEnumFunc then
        return false
    end

    return isEnumFunc(target)
end

oop.isdelegate = function(target)
    if not target then
        return false
    end

    local isDelegateFunc = target.IsDelegate
    if not isDelegateFunc then
        return false
    end
    return isDelegateFunc(target)
end

oop.iskindof = function(target, baseClass)
    if not oop.isclassorinstance(target) or not oop.isclass(baseClass) then
        return false
    end

    return target:IsKindOf(baseClass)
end

oop.Logger = require('DotLua/OOP/Logger')
oop.info = oop.Logger.Info
oop.warning = oop.Logger.Warning
oop.error = oop.Logger.Error

return oop
