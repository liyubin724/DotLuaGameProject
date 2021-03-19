local oop = require('DotLua/OOP/oop')
local Matcher = oop.using('DotLua/ECS/Matchers/Matcher')

local HasComponentMatcher =
    oop.class(
    'HasComponentMatcher',
    function(self, name, componentClass)
        if oop.isDebug then
            if not self.componentClass then
                oop.error('HasComponentMatcher', '')
                return
            elseif self.componentClass:GetType() ~= oop.ObjectType.Class or self.componentClass:IsKindOf(Component) then
                oop.error('HasComponentMatcher', '')
                return
            end
        end

        self.componentClass = componentClass
    end,
    Matcher
)

function HasComponentMatcher:IsMatch(entityObj)
    if not self.componentClass then
        return false
    end

    return entityObj:HasComponent(self.componentClass)
end

return HasComponentMatcher
