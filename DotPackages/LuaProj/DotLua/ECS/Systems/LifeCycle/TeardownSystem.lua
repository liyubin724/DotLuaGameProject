local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local TeardownSystem =
    oop.class(
    'TearDownSystem',
    function(self)
    end,
    System
)

function TeardownSystem:DoTeardown()
    oop.error('ECS', 'InitializeSystem:DoInitialize->this is a abstract class')
end

return TeardownSystem
