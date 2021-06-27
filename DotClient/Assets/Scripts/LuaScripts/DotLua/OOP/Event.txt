local ObjectType = require('DotLua/OOP/ObjectType')
local Delegate = require('DotLua/OOP/Delegate')
local class = require('DotLua/OOP/class')

local tremove = table.remove
local tinsert = table.insert
local tclear = table.clear

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
            tinsert(self.listeners, Delegate(receiver, func))
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
                if listener:GetReceiver() == receiver and listener:GetFunc() == func then
                    tremove(self.listeners, i)
                end
            end
        end
    end

    return self
end

Event._type = ObjectType.Event

function Event:Add(receiver, func)
    tinsert(self.listeners, Delegate(receiver, func))
end

function Event:Remove(receiver, func)
    for i = #(self.listeners), 1, -1 do
        local listener = self.listeners[i]
        if listener:GetReceiver() == receiver and listener:GetFunc() == func then
            tremove(self.listeners, i)
        end
    end
end

function Event:Clear()
    tclear(self.listeners)
end

function Event:Invoke(...)
    for _, listener in ipairs(self.listeners) do
        listener:ActionInvoke(...)
    end
end

return Event
