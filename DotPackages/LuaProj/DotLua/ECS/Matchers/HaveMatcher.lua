local oop = require('DotLua/OOP/oop')
local DebugLogger = require('DotLua/Log/DebugLogger')

local Component = oop.using('DotLua/ECS/Components/Component')
local Matcher = oop.using('DotLua/ECS/Matchers/Matcher')

local HaveMatcher =
    oop.class(
    'HaveMatcher',
    function(self, componentClass)
        self.componentClass = componentClass
    end,
    Matcher
)

function HaveMatcher:IsMatch(entityObj)
    if oop.isDebug then
        if not self.componentClass then
            DebugLogger.Error('HaveMatcher', '')
            return false
        elseif self.componentClass:GetType() ~= oop.ObjectType.Class or self.componentClass:IsKindOf(Component) then
            DebugLogger.Error('HaveMatcher', '')
            return false
        end
    end

    return entityObj:HasComponent(self.componentClass)
end

return HaveMatcher
