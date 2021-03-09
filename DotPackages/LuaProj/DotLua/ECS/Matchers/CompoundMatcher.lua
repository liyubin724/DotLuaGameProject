local oop = require('DotLua/OOP/oop')
local DebugLogger = require('DotLua/Log/DebugLogger')

local Matcher = oop.using('DotLua/ECS/Matchers/Matcher')
local List = oop.using('DotLua/Generic/Collections/List')

local CompoundMatcher =
    oop.class(
    'CompoundMatcher',
    function(self, ...)
        self.innerMatchers = List()
        for i = 1, select('#', ...), 1 do
            local matcher = select(i, ...)

            if oop.isDebug then
                if not matcher then
                    DebugLogger.Error("CompoundMatcher","")
                    self.innerMatchers:Clear()
                    break
                elseif not matcher:IsKindOf(Matcher) then
                    DebugLogger.Error("CompoundMatcher","")
                    self.innerMatchers:Clear()
                    break
                end
            end

            self.innerMatchers:Add(matcher)
        end
    end,
    Matcher
)

return CompoundMatcher
