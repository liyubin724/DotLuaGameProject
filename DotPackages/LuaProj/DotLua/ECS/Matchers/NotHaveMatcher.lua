local oop = require('DotLua/OOP/oop')
local DebugLogger = require('DotLua/Log/DebugLogger')

local Component = oop.using('DotLua/ECS/Components/Component')
local Matcher = oop.using('DotLua/ECS/Matchers/Matcher')

local NotHaveMatcher =
    oop.class(
    'NotHaveMatcher',
    function(self, componentClass)
        self.componentClass = componentClass
    end,
    Matcher
)

function NotHaveMatcher:IsMatch(entityObj)
    if oop.isDebug then
        if not self.componentClass then
            DebugLogger.Error('NotHaveMatcher', '')
            return false
        elseif self.componentClass:GetType() ~= oop.ObjectType.Class or self.componentClass:IsKindOf(Component) then
            DebugLogger.Error('NotHaveMatcher', '')
            return false
        end
    end

    return not entityObj:HasComponent(self.componentClass)
end

return NotHaveMatcher
