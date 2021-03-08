local System = using('DotLua/ECS/Systems/System')

local InitializeSystem =
    class(
    'InitializeSystem',
    function(self)
    end,
    System
)

function InitializeSystem:Initialize()

end

return InitializeSystem
