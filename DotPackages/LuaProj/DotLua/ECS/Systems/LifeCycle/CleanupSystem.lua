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
end

return CleanupSystem
