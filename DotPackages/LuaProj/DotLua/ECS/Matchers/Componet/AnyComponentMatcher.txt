local oop = require('DotLua/OOP/oop')
local MulComponentMatcher = oop.using('DotLua/ECS/Matchers/Component/MulComponentMatcher')

local AnyComponentMatcher =
    oop.class(
    'AnyComponentMatcher',
    function(self, name, ...)
    end,
    MulComponentMatcher
)

function AnyComponentMatcher:IsMatch(entityObject)
    if not self.componentClasses or #(self.componentClasses) == 0 then
        return false
    end

    return entityObject:HasAnyComponent(self.componentClasses)
end

return AnyComponentMatcher
