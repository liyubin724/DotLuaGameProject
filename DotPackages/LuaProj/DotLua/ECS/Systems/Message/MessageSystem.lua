local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local MessageSystem =
    oop.class(
    'MessageSystem',
    function(self,dispatcher)
        self.dispatcher = dispatcher
    end,
    System
)

function MessageSystem:DoInitialize()
    local eventNames = self:ListEventNames()
    if eventNames and #(eventNames) > 0 and self.dispatcher then
        for _, name in ipairs(eventNames) do
            self.dispatcher:RegistListener(name,self,self.HandleEvent)
        end
    end
end

function MessageSystem:ListEventNames()
    return nil
end

function MessageSystem:HandleEvent(eventName,...)
end

function MessageSystem:DoTeardown()
    local eventNames = self:ListEventNames()
    if eventNames and #(eventNames) > 0 and self.dispatcher then
        for _, name in ipairs(eventNames) do
            self.dispatcher:UnregistListener(name,self,self.HandleEvent)
        end
    end
end

return MessageSystem
