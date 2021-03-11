local ObjectType = require('DotLua/OOP/ObjectType')

local ObjectMeta = {}
ObjectMeta.__call = function(class,...)
    local instance = setmetatable({}, class)
    instance._isInstance = true

    if class._ctor then
        class._ctor(class, instance, ...)
    end
    return instance
end

local Object = setmetatable({}, ObjectMeta)
Object.__index = Object

Object._base = nil
Object._className = 'Object'
Object._type = ObjectType.Class
Object._isInstance = false

Object._ctor = function(instance, ...)
end

function Object:GetClassName()
    return self._className
end

function Object:GetBaseClass()
    return self._base
end

function Object:GetType()
    return self._type
end

function Object:GetIsInstance()
    return self._isInstance
end

function Object:IsClass()
    return self._type == ObjectType.Class
end

function Object:IsEnum()
    return self._type == ObjectType.Enum
end

function Object:IsDelegate()
    return self._type == ObjectType.Delegate
end

function Object:IsEvent()
    return self._type == ObjectType.Event
end

function Object:IsKindOf(baseClass)
    local c = self
    while c do
        if c == baseClass then
            return true
        end

        c = c._base
    end
    return false
end

function Object:ToString()
    return self._className
end

return Object
