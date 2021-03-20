local ObjectType = require('DotLua/OOP/ObjectType')
local class = require('DotLua/OOP/class')

local Delegate =
    class(
    'Delegate',
    function(self, receiver, func)
        self.receiver = receiver
        self.func = func
    end
)

Delegate.__eq = function(d1, d2)
    if d1 == d2 then
        return true
    elseif d1 ~= nil and d2 ~= nil then
        return d1.receiver == d2.receiver and d1.func == d2.func
    else
        return false
    end
end

Delegate._type = ObjectType.Delegate

function Delegate:GetReceiver()
    return self.receiver
end

function Delegate:GetFunc()
    return self.func
end

function Delegate:ActionInvoke(...)
    if self.func then
        if self.receiver then
            self.func(self.receiver, ...)
        else
            self.func(...)
        end
    end
end

function Delegate:FuncInvoke(...)
    if self.func then
        if self.receiver then
            return self.func(self.receiver, ...)
        else
            return self.func(...)
        end
    end

    return nil
end

function Delegate:ToString()
    return string.format('{Delegate:{receiver:%s,func:%s}}', tostring(self.receiver), tostring(self.func))
end

return Delegate
