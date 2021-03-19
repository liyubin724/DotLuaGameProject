local oop = require('DotLua/OOP/oop')
local CombinedMatcher = oop.using('DotLua/ECS/Matchers/Combined/CombinedMatcher')

local AnyOfMatcher =
    oop.class(
    'AnyOfMatcher',
    function(self, name, ...)
    end,
    CombinedMatcher
)

function AnyOfMatcher:IsMatch(entityObj)
    if not self.matchers or #(self.matchers) == 0 then
        return false
    end

    for _, matcher in ipairs(self.matchers) do
        if matcher:IsMatch(entityObj) then
            return true
        end
    end

    return false
end

return AnyOfMatcher
