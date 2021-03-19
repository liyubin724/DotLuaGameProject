local ObjectType = require('DotLua/OOP/ObjectType')
local class = require('DotLua/OOP/class')

local tisarray = table.isarray
local tkeys = table.keys
local tvalues = table.values

local Enum =
    class(
    'Enum',
    function(self, name, values)
        self._isInstance = false

        self._name = name
        self._values = {}
        if values then
            if tisarray(values) then
                for i, v in ipairs(values) do
                    self._values[v] = i
                end
            else
                for k, v in pairs(values) do
                    self._values[k] = v
                end
            end
        end
    end
)

Enum.__index = function(e, k)
    local v = e._values[k]
    if not v then
        v = Enum[k]
    end

    return v
end

Enum.__tostring = function(e)
    local info = string.format('%s:{', e._name)
    for k, v in pairs(e._values) do
        info = info .. string.format('%s:%d,', k, v)
    end
    info = info .. '}'
    return info
end

Enum._type = ObjectType.Enum

function Enum:GetName()
    return self._name
end

function Enum:GetNames()
    return tkeys(self._values)
end

function Enum:GetValues()
    return tvalues(self._values)
end

function Enum:GetNameByValue(value)
    for k, v in pairs(self._values) do
        if v == value then
            return k
        end
    end

    return nil
end

function Enum:ToString()
    return tostring(self)
end

return Enum
