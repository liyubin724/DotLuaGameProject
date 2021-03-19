local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local InitializeSystem =
    oop.class(
    'InitializeSystem',
    function(self)
    end,
    System
)

function InitializeSystem:DoInitialize()
    oop.error("ECS","InitializeSystem:DoInitialize->this is a abstract class")
end

return InitializeSystem
