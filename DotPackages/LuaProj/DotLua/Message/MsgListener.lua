local oop = require('DotLua/OOP/oop')

local MsgListener =
    oop.class(
    'MsgListener',
    function(self)
        self.name = nil
        self.receiver = nil
        self.func = nil
        self.userdata = nil
    end
)

MsgListener.__eq = function(listener1, listener2)
    if listener1 == nil and listener2 == nil then
        return true
    end
    if listener1 == nil or listener2 == nil then
        return false
    end

    return listener1.name == listener2.name and listener1.receiver == listener2.receiver and
        listener1.func == listener2.func
end

function MsgListener:GetName()
    return self.name
end

function MsgListener:GetReceiver()
    return self.receiver
end

function MsgListener:GetFunc()
    return self.func
end

function MsgListener:GetUserdata()
    return self.userdata
end

function MsgListener:Set(name, receiver, func, userdata)
    self.name = name
    self.receiver = receiver
    self.func = func
    self.userdata = userdata
end

function MsgListener:Invoke(...)
    if self.func then
        if self.receiver then
            if self.userdata then
                self.func(self.receiver, self.name, self.userdata, ...)
            else
                self.func(self.receiver, self.name, ...)
            end
        else
            if self.userdata then
                self.func(self.name, self.userdata, ...)
            else
                self.func(self.name, ...)
            end
        end
    end
end

function MsgListener:OnRelease()
    self.name = nil
    self.receiver = nil
    self.func = nil
    self.userdata = nil
end

return MsgListener
