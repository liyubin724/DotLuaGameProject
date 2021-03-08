local System = using('DotLua/ECS/Systems/System')

local MessageSystem =
    class(
    'EventSystem',
    function(self)
    end,
    System
)

function MessageSystem:ListInterestEvents()
    return nil
end

function MessageSystem:HandleEvent(eventName)
end

return MessageSystem
