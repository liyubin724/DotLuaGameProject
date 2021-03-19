local oop = require('DotLua/OOP/oop')
local FrameSystem = oop.using('DotLua/ECS/Systems/LifeCycle/FrameSystem')

local ExecuteSystem =
    oop.class(
    'ExecuteSystem',
    function(self, group, frameInterval)
        self.frameInterval = frameInterval or 1
        self.group = group

        self.passedFrame = 0
    end,
    FrameSystem
)

function ExecuteSystem:GetFrameInterval()
    return self.frameInterval
end

function ExecuteSystem:DoFrame()
    self.passedFrame = self.passedFrame + 1
    if self.passedFrame == self.frameInterval then
        self:Execute()
        self.passedFrame = 0
    end
end

function ExecuteSystem:Execute()
    oop.error('ECS', 'ExecuteSystem:Execute->this is a abstract class')
end

return ExecuteSystem
