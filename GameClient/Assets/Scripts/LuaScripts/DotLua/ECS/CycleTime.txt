local oop = require('DotLua/OOP/oop')

local CycleTime =
    oop.class(
    'CycleTime',
    function(self)
        self.deltaTime = 0
        self.unscaleDeltaTime = 0

        self.timeScale = 1.0

        self.frameTime = 0.1
        self.frameIndex = 0
    end
)

function CycleTime:GetDeltaTime()
    return self.deltaTime * self.timeScale
end

function CycleTime:GetUnscaleDeltaTime()
    return self.unscaleDeltaTime
end

function CycleTime:GetTimeScale()
    return self.timeScale
end

function CycleTime:SetTimeScale(timeScale)
    self.timeScale = timeScale
end

function CycleTime:SetFrameTime(frameTime)
    self.frameTime = frameTime
end

function CycleTime:GetFrameTime()
    return self.frameTime
end

function CycleTime:GetFrameIndex()
    return self.frameIndex
end

function CycleTime:DoUpdate(deltaTime, unscaleDeltaTime)
    self.deltaTime = deltaTime
    self.unscaleDeltaTime = unscaleDeltaTime

    self.frameIndex = self.frameIndex + 1
end

return CycleTime
