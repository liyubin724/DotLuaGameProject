local Object = require('DotLua/OOP/Object')
local ObjectType = require('DotLua/OOP/ObjectType')

local Delegate = {}
Delegate.__index = Delegate
Delegate.__eq = function(d1, d2)
    if d1 == d2 then
        return true
    elseif d1 ~= nil and d2 ~= nil then
        return d1.receiver == d2.receiver and d1.func == d2.func
    else
        return false
    end
end

Delegate.__call = function(_, receiver, func)
    local delegate = setmetatable({}, Delegate)
    Delegate._ctor(delegate, receiver, func)

    return delegate
end

Delegate._base = Object
Delegate._className = 'Delegate'
Delegate._type = ObjectType.Delegate
Delegate._ctor = function(obj, receiver, func)
    obj.receiver = receiver
    obj.func = func
end

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

setmetatable(Delegate, Object)

return Delegate
