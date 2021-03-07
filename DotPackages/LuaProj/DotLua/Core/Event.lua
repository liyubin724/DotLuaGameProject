local EventListener =
    class(
    'EventListener',
    function(self,receiver,func)
        self.receiver = receiver
        self.func = func
    end
)

function EventListener:Invoke(...)
    if self.func then
        if self.receiver then
            self.func(self.receiver, ...)
        else
            self.func(...)
        end
    end
end

function EventListener:IsEqualTo(receiver,func)
    return self.receiver == receiver and self.func == func
end

local Event =
    class(
    'Event',
    function(self)
        self.listeners = {}
    end
)

Event.__len = function(self)
    return #(self.listeners)
end

Event.__add = function(self, data)
    if data and type(data) == 'table' then
        local dataLen = #(data)

        local receiver = nil
        local func = nil

        if dataLen == 2 then
            receiver = data[1]
            func = data[2]
        elseif dataLen == 1 then
            func = data[1]
        end

        if func and type(func) == 'function' then
            table.insert(self.listeners, EventListener(receiver,func))
        end
    end
    return self
end

Event.__sub = function(self, data)
    if data and type(data) == 'table' then
        local dataLen = #(data)

        local receiver = nil
        local func = nil

        if dataLen == 2 then
            receiver = data[1]
            func = data[2]
        elseif dataLen == 1 then
            func = data[1]
        end

        if func and type(func) == 'function' then
            for i = #(self.listeners), 1, -1 do
                local listener = self.listeners[i]
                if listener:IsEqualTo(receiver,func) then
                    table.remove(self.listeners, i)
                end
            end
        end
    end

    return self
end

function Event:Invoke(...)
    for _, listener in ipairs(self.listeners) do
        listener:Invoke(...)
    end
end

function Event:ClearAll()
    table.clear(self.listeners)
end

return Event
