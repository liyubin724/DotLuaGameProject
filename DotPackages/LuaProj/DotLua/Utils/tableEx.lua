local table = _G.table
local next = _G.next
local type = _G.type
local tostring = _G.tostring
local pairs = _G.pairs
local ipairs = _G.ipairs

function table.isempty(tbl)
    if tbl == nil or next(tbl) == nil then
        return true
    else
        return false
    end
end

function table.isarray(tbl)
    if type(tbl) ~= 'table' then
        return false
    end
    local i = 0
    for _ in pairs(tbl) do
        i = i + 1
        if tbl[i] == nil and tbl[tostring(i)] == nil then
            return false
        end
    end
    return true
end

function table.length(tbl)
    if type(tbl) ~= 'table' then
        return 0
    end

    local len = 0
    for _, _ in pairs(tbl) do
        len = len + 1
    end
    return len
end

function table.arrlength(tbl)
    if type(tbl) ~= 'table' then
        return 0
    end
    return #(tbl)
end

function table.keys(tbl)
    local t = {}
    if type(tbl) ~= 'table' then
        return t
    end

    local n = 0
    for key, _ in pairs(tbl) do
        n = n + 1
        t[n] = tostring(key)
    end

    return t
end

function table.values(tbl)
    local t = {}
    if type(tbl) ~= 'table' then
        return t
    end

    local n = 0

    for _, value in pairs(tbl) do
        n = n + 1
        t[n] = value
    end

    return t
end

function table.containskey(tbl, key)
    if type(tbl) ~= 'table' then
        return false
    end

    return tbl[key] ~= nil
end

function table.containsvalue(tbl, value)
    if type(tbl) ~= 'table' then
        return false
    end

    for _, v in pairs(tbl) do
        if v == value then
            return true
        end
    end
    return false
end

function table.removevalue(tbl,value)
    if type(tbl) ~= 'table' then
        return false
    end

    for k, v in pairs(tbl) do
        if v == value then
            tbl[k] = nil
            return true
        end
    end

    return false
end

function table.foreach(tbl, func)
    if type(tbl) ~= 'table' then
        return
    end

    for _, value in pairs(tbl) do
        func(value)
    end
end

function table.any(tbl, checkFunc)
    if type(tbl) ~= 'table' then
        return false
    end

    for _, value in pairs(tbl) do
        if checkFunc(value) then
            return true
        end
    end
    return false
end

function table.all(tbl, checkFunc)
    if type(tbl) ~= 'table' then
        return false
    end

    for key, value in pairs(tbl) do
        if not checkFunc(key, value) then
            return false
        end
    end
    return true
end

function table.indexof(tbl, value)
    if type(tbl) ~= 'table' then
        return -1
    end

    for i, v in ipairs(tbl) do
        if v == value then
            return i
        end
    end
    return -1
end

function table.key(tbl, value)
    if type(tbl) ~= 'table' then
        return nil
    end

    for k, v in pairs(tbl) do
        if v == value then
            return k
        end
    end
    return nil
end

function table.copy(tbl)
    if type(tbl) ~= 'table' then
        return nil
    end

    local rtbl = {}
    for _, item in ipairs(tbl) do
        table.insert(rtbl, item)
    end
    return rtbl
end

function table.copyto(sourceTbl, destinationTbl)
    if type(sourceTbl) ~= 'table' then
        return
    end

    destinationTbl = {}
    for _, item in ipairs(sourceTbl) do
        table.insert(destinationTbl, item)
    end
end

function table.clear(tbl)
    for k, _ in pairs(tbl) do
        tbl[k] = nil
    end
end

function table.tostring(tbl)
    if type(tbl) ~= 'table' then
        return tostring(tbl)
    end

    local serpent = require('DotLua/Tools/serpent')
    return serpent.dump(tbl)
end
