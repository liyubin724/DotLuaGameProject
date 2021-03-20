local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local FrameSystem =
    oop.class(
    'FrameSystem',
    function(self)
    end,
    System
)

function FrameSystem:DoFrame()
    oop.error('ECS', 'FrameSystem:DoFrame->this is a abstract class')
end

return FrameSystem
