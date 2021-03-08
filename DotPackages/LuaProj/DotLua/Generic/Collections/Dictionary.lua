local tkeys = table.keys
local tvalues = table.values
local tisempty = table.isempty

local Dictionary =
    class("Dictionary",
    function(self)
        self.dic = {}
    end
)

Dictionary.__len = function(self)
    return self:Count()
end

function Dictionary:Count()
    return #(tkeys(self.dic))
end

function Dictionary:Keys()
    return tkeys(self.dic)
end

function Dictionary:Values()
    return tvalues(self.dic)
end

function Dictionary:IsEmpty()
    return tisempty(self.dic)
end

function Dictionary:ContainsKey(key)
    return self.dic[key] ~= nil
end

function Dictionary:Get(key)
    return self.dic[key]
end

function Dictionary:Add(key, value)
    if key then
        local v = self.dic[key]
        if not v then
            self.dic[key] = value
        else
            error('')
        end
    else
        error('')
    end
end

function Dictionary:AddOrUpdate(key, value)
    if key then
        self.dic[key] = value
    else
        error('')
    end
end

function Dictionary:Update(key, value)
    if key then
        local v = self.dic[key]
        if v then
            self.dic[key] = value
        else
            error('')
        end
    else
        error('')
    end
end

function Dictionary:Remove(key)
    if self:ContainsKey(key) then
        self.dic[key] = nil
    end
end

function Dictionary:GetEnumerator()
    local index = 0
    local count = self:Count()
    local keys = self:Keys()

    return function()
        index = index + 1
        if index <= count then
            local key = keys[index]
            local value = self.dic[key]
            return key, value
        end
        return nil, nil
    end
end

function Dictionary:ForEach(func)
    if not func or type(func) ~= 'function' then
        error('func is empty')
        return
    end

    for key, value in pairs(self.dic) do
        func(key, value)
    end
end

function Dictionary:Find(compareFunc)
    if not compareFunc or type(compareFunc) ~= 'function' then
        error('compareFunc is empty')
        return nil,nil
    end

    for key, value in pairs(self.dic) do
        if compareFunc(key, value) then
            return key, value
        end
    end
    return nil, nil
end

return Dictionary
