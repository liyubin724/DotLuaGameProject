local isDebug = _G.isDebug or true
local ObjectType = require('DotLua/ObjectType')

local meta = {}
meta.__index = function(t, k)
    return t._values[k]
end

meta.__newindex = function(t, k, v)
    error(string.format('add new kv(%s) in to the enum is not allowed', tostring(k)))
end

meta.__tostring = function(e)
    local info = string.format("%s:{",e._typeName)
    for k, v in pairs(e._values) do
        info = info..string.format("%s:%d,",k,v)
    end
    info = info.."}"
    return info
end

function enum(name, values)
    local e = {}
    e._typeName = name
    e._type = ObjectType.Enum
    e._values = {}

    local isValid = true
    if isDebug then
        if values and type(values) == 'table' then
            if table.isarray(values) then
                isValid =
                    table.all(
                    values,
                    function(k, v)
                        return v and type(v) == 'string'
                    end
                )
            else
                isValid =
                    table.all(
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
        if table.isarray(values) then
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

    setmetatable(e, meta)
    return e
end
