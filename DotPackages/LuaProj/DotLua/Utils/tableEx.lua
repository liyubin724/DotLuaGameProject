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

function table.contains(tbl, key)
    if type(tbl) ~= 'table' then
        return false
    end

    return tbl[key] ~= nil
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
        if not checkFunc(key,value) then
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

end

function table.deepcopy(tbl)

end

function table.clear(tbl)
    for k, _ in pairs(tbl) do
        tbl[k] = nil
    end
end
