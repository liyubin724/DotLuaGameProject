local oop = require('DotLua/OOP/oop')
local MulComponentMatcher = oop.using('DotLua/ECS/Matchers/Component/MulComponentMatcher')

local NoneComponentMatcher =
    oop.class(
    'NoneComponentMatcher',
    function(self, name, ...)
    end,
    MulComponentMatcher
)

function NoneComponentMatcher:IsMatch(entityObject)
    if not self.componentClasses or #(self.componentClasses) == 0 then
        return false
    end

    return not entityObject:HasAnyComponent(self.componentClasses)
end

return NoneComponentMatcher
