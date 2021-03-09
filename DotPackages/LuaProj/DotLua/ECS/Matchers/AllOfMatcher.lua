local oop = require('DotLua/OOP/oop')
local CompoundMatcher = oop.using('DotLua/ECS/Matchers/CompoundMatcher')

local AllOfMatcher =
    oop.class(
    'AllOfMatcher',
    function(self, ...)
    end,
    CompoundMatcher
)

function AllOfMatcher:IsMatch(entityObj)
    for _, matcher in ipairs(self.innerMatchers) do
        if not matcher:IsMatch(entityObj) then
            return false
        end
    end

    return true
end

return AllOfMatcher
