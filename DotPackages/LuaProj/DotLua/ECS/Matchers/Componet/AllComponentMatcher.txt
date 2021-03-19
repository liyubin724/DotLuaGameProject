local oop = require('DotLua/OOP/oop')
local MulComponentMatcher = oop.using('DotLua/ECS/Matchers/Component/MulComponentMatcher')

local AllComponentMatcher =
    oop.class(
    'AllComponentMatcher',
    function(self, name, ...)
    end,
    MulComponentMatcher
)

function AllComponentMatcher:IsMatch(entityObject)
    if not self.componentClasses or #(self.componentClasses) == 0 then
        return false
    end

    return entityObject:HasComponents(self.componentClasses)
end

return AllComponentMatcher
