local oop = require('DotLua/OOP/oop')
local CombinedMatcher = oop.using('DotLua/ECS/Matchers/Combined/CombinedMatcher')

local NoneOfMatcher =
    oop.class(
    'NoneOfMatcher',
    function(self, name, ...)
    end,
    CombinedMatcher
)

function NoneOfMatcher:IsMatch(entityObj)
    if not self.matchers or #(self.matchers) == 0 then
        return false
    end

    for _, matcher in ipairs(self.innerMatchers) do
        if matcher:IsMatch(entityObj) then
            return false
        end
    end

    return true
end

return NoneOfMatcher
