local oop = require('DotLua/OOP/oop')
local Component = oop.using('DotLua/ECS/Components/Component')
local Matcher = oop.using('DotLua/ECS/Matchers/Matcher')

local NotComponentMatcher =
    oop.class(
    'NotComponentMatcher',
    function(self, name, componentClass)
        if oop.isDebug then
            if not self.componentClass then
                oop.error('NotComponentMatcher', '')
                return
            elseif self.componentClass:GetType() ~= oop.ObjectType.Class or self.componentClass:IsKindOf(Component) then
                oop.error('NotComponentMatcher', '')
                return
            end
        end

        self.componentClass = componentClass
    end,
    Matcher
)

function NotComponentMatcher:IsMatch(entityObj)
    if not self.componentClass then
        return false
    end

    return not entityObj:HasComponent(self.componentClass)
end

return NotComponentMatcher
