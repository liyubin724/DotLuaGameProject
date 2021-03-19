local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local LateUpdateSystem =
    oop.class(
    'LateUpdateSystem',
    function(self)
    end,
    System
)

function LateUpdateSystem:DoLateUpdate()
    oop.error('ECS', 'LateUpdateSystem:DoLateUpdate->this is a abstract class')
end

return LateUpdateSystem
