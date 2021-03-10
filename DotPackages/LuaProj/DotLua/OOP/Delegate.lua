local Delegate =
    class(
    'Delegate',
    function(self,receiver,func)
        self.receiver = receiver
        self.func = func
    end
)

Delegate.__eq = function(d1,d2)
    if d1 == nil and d2 == nil then
        return true
    elseif d1 ~= nil and d2 ~= nil then
        return d1.receiver == d2.receiver and d1.func == d2.func
    else
        return false
    end
end

function Delegate:Bind(receiver,func)
    self.receiver = receiver
    self.func = func
end

function Delegate:Unbind()
    self.receiver = nil
    self.func = nil
end


function Delegate:GetReceiver()
    return self.receiver
end

function Delegate:GetFunc()
    return self.func
end

function Delegate:Invoke(...)
    if self.func then
        if self.receiver then
            return self.func(self.receiver, ...)
        else
            return self.func(...)
        end
    end

    return nil
end

function Delegate:Equal(otherDelegate)
    if self == otherDelegate then
        return true
    else
        return self.receiver == otherDelegate.receiver and self.func == otherDelegate.func
    end
end

return Delegate
