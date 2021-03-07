local isDebug = _G.isDebug or true
local ObjectType = require('DotLua/ObjectType')
local Object = require('DotLua/Object')

local call = function(classTbl, ...)
    if isDebug then
        if not classTbl:IsClass() then
            error('it is not a class')
            return nil
        end
    end

    local obj = setmetatable({}, classTbl)
    obj._type = ObjectType.Instance
    classTbl._ctor(obj, ...)
    return obj
end

function class(name, ctor, base)
    if isDebug then
        if type(name) ~= 'string' then
            error('the name of the class should be str!')
            return nil
        end
        if ctor == nil or type(ctor) ~= 'function' then
            error('the ctor of the class should be a function')
            return nil
        end
        if base ~= nil and type(base) ~= 'table' then
            error('the base of the clas should be a class')
            return nil
        end
    end

    if not base then
        base = Object
    end

    local c = {}
    c._base = base
    c._typeName = name
    c._type = ObjectType.Class
    c.__index = c

    c._ctor = function(obj, ...)
        if base then
            base._ctor(obj, ...)
        end

        ctor(obj, ...)
    end

    local meta = {}
    meta.__index = function(obj, k)
        if k == "new" then
            return call
        end
        if base then
            return base[k]
        end
        return nil
    end
    meta.__call = call

    setmetatable(c, meta)

    return c
end
