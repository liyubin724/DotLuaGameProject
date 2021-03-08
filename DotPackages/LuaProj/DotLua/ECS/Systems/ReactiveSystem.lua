local System = using('DotLua/ECS/Systems/System')

local ReactiveSystem =
    class(
    'ReactiveSystem',
    function(self)
    end,
    System
)

function ReactiveSystem:Active()
end

function ReactiveSystem:Deactive()
end

function ReactiveSystem:Clear()
end

return ReactiveSystem
