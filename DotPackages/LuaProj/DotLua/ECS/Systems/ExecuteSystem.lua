local System = using('DotLua/ECS/Systems/System')

local ExecuteSystem =
    class(
    'ExecuteSystem',
    function(self)
    end,
    System
)

function ExecuteSystem:Execute()
end

return ExecuteSystem
