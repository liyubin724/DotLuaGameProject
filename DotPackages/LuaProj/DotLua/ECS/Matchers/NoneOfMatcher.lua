local oop = require('DotLua/OOP/oop')
local CompoundMatcher = oop.using('DotLua/ECS/Matchers/CompoundMatcher')

local NoneOfMatcher =
    oop.class(
    'NoneOfMatcher',
    function(self, ...)
    end,
    CompoundMatcher
)

function NoneOfMatcher:IsMatch(entityObj)
    for _, matcher in ipairs(self.innerMatchers) do
        if matcher:IsMatch(entityObj) then
            return false
        end
    end

    return true
end

return NoneOfMatcher