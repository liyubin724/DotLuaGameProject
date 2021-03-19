local oop = require('DotLua/OOP/oop')
local CombinedMatcher = oop.using('DotLua/ECS/Matchers/Combined/CombinedMatcher')

local AllOfMatcher =
    oop.class(
    'AllOfMatcher',
    function(self, name, ...)
    end,
    CombinedMatcher
)

function AllOfMatcher:IsMatch(entityObj)
    if not self.matchers or #(self.matchers) == 0 then
        return false
    end

    for _, matcher in ipairs(self.matchers) do
        if not matcher:IsMatch(entityObj) then
            return false
        end
    end

    return true
end

return AllOfMatcher
