local System = using('DotLua/ECS/Systems/System')

local TearDownSystem =
    class(
    'TearDownSystem',
    function(self)
    end,
    System
)

function TearDownSystem:TearDown()
end

return TearDownSystem
