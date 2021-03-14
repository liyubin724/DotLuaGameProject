local oop = require('DotLua/OOP/oop')
local Matcher = oop.using('DotLua/ECS/Matchers/Matcher')

local tinsert = table.insert
local select = select

local CombinedMatcher =
    oop.class(
    'CombinedMatcher',
    function(self, name, ...)
        local matchers = {}
        local length = select('#', ...)
        if length > 0 then
            for i = 1, length, 1 do
                local matcherClass = select(i, ...)
                if oop.iskindof(matcherClass, Matcher) then
                    tinsert(matchers, matcherClass)
                else
                    oop.error('MulComponentMatcher', 'it is not a subclass of Component')
                    return
                end
            end
        end

        self.matchers = matchers
    end,
    Matcher
)

function CombinedMatcher:IsMatch(entityObject)
    oop.error('CombinedMatcher', 'This is abstract class')
    return false
end

return CombinedMatcher
