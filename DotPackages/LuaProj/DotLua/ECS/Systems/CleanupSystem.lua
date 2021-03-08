local System = using('DotLua/ECS/Systems/System')

local CleanupSystem =
    class(
    'CleanupSystem',
    function(self)
    end,
    System
)

function CleanupSystem:Cleanup()

end

return CleanupSystem
