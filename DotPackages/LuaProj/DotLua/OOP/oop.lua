require('DotLua/Utils/stringEx')
require('DotLua/Utils/tableEx')

local Delegate = require("DotLua/OOP/Delegate")

local tisarray = table.isarray
local tall = table.all

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

local call = function(classTbl, ...)
    if oop.isDebug then
        if not classTbl:IsClass() then
            error('it is not a class')
            return nil
        end
    end

    local obj = setmetatable({}, classTbl)
    obj._type = oop.ObjectType.Instance
    classTbl._ctor(obj, ...)
    return obj
end

oop.class = function(name, ctor, base)
    if oop.isDebug then
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
        base = oop.Object
    end

    local c = {}
    c._base = base
    c._typeName = name
    c._type = oop.ObjectType.Class
    c.__index = c

    c._ctor = function(obj, ...)
        if base then
            base._ctor(obj, ...)
        end

        ctor(obj, ...)
    end

    local meta = {}
    meta.__index = function(obj, k)
        if k == 'new' then
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

local enumMeta = {
    __index = function(t, k)
        return t._values[k]
    end,
    __newindex = function(t, k, v)
        error(string.format('add new kv(%s) in to the enum is not allowed', tostring(k)))
    end,
    __tostring = function(e)
        local info = string.format('%s:{', e._typeName)
        for k, v in pairs(e._values) do
            info = info .. string.format('%s:%d,', k, v)
        end
        info = info .. '}'
        return info
    end
}

oop.enum = function(name, values)
    local e = {}
    e._typeName = name
    e._type = oop.ObjectType.Enum
    e._values = {}

    local isValid = true
    if oop.isDebug then
        if values and type(values) == 'table' then
            if tisarray(values) then
                isValid =
                    tall(
                    values,
                    function(k, v)
                        return v and type(v) == 'string'
                    end
                )
            else
                isValid =
                    tall(
                    values,
                    function(k, v)
                        return v and k and type(k) == 'string' and type(v) == 'number'
                    end
                )
            end
        else
            isValid = false
        end
    end

    if isValid then
        if tisarray(values) then
            for i, v in ipairs(values) do
                e._values[v] = i
            end
        else
            for k, v in pairs(values) do
                e._values[k] = v
            end
        end
    else
        error('')
    end

    setmetatable(e, enumMeta)
    return e
end

oop.delegate = function(receiver,func)
    local d = {}
    d.receiver = receiver
    d.func = func

    setmetatable(d, Delegate)
    return d
end

return oop