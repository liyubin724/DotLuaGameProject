local ObjectType = require('DotLua/ObjectType')

local Object = {}
Object.__index = Object

Object._base = nil
Object._typeName = 'Object'
Object._type = ObjectType.Class
Object._ctor = function(obj, ...)
end

function Object:GetClassName()
    return self._typeName
end

function Object:IsClass()
    return self._type == ObjectType.Class
end

function Object:IsInstance()
    return self._type == ObjectType.Instance
end

function Object:GetClass()
    if self:IsInstance() then
        return getmetatable(self)
    elseif self:IsClass() then
        return self
    end
    return nil
end

function Object:IsKindOf(baseClass)
    local c = self:GetClass()
    while c do
        if c == baseClass then
            return true
        end

        c = c._base
    end
    return false
end

function Object:ToString()
    return self.__typeName
end

return Object
