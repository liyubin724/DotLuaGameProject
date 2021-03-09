local oop = require('DotLua/OOP/oop')
local CompoundMatcher = oop.using('DotLua/ECS/Matchers/CompoundMatcher')

local AnyOfMatcher =
    oop.class(
    'AnyOfMatcher',
    function(self, ...)
    end,
    CompoundMatcher
)

function AnyOfMatcher:IsMatch(entityObj)
    for _, matcher in ipairs(self.innerMatchers) do
        if matcher:IsMatch(entityObj) then
            return true
        end
    end

    return false
end

return AnyOfMatcher