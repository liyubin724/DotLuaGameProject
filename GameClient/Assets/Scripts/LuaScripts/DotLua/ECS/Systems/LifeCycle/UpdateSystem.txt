local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local UpdateSystem =
    oop.class(
    'UpdateSystem',
    function(self)
    end,
    System
)

function UpdateSystem:DoUpdate()
    oop.error('ECS', 'UpdateSystem:DoUpdate->this is a abstract class')
end

return UpdateSystem
