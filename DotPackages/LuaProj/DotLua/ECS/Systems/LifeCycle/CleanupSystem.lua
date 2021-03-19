local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local CleanupSystem =
    oop.class(
    'CleanupSystem',
    function(self)
    end,
    System
)

function CleanupSystem:DoCleanup()
    oop.error('ECS', 'CleanupSystem:DoCleanup->this is a abstract class')
end

return CleanupSystem
