local oop = require('DotLua/OOP/oop')
local Matcher = oop.using('DotLua/ECS/Matchers/Matcher')
local Component = oop.using("DotLua/ECS/Components/Component")

local select = select
local tinsert = table.insert

local MulComponentMatcher =
    oop.class(
    'MulComponentMatcher',
    function(self, name, ...)
        local compClasses = {}

        local length = select("#",...)
        if length>0 then
            for i = 1, length, 1 do
                local componentClass = select(i,...)
                if oop.iskindof(componentClass,Component) then
                    tinsert(compClasses,componentClass)
                else
                    oop.error("MulComponentMatcher","it is not a subclass of Component")
                    return
                end
            end
        end

        self.componentClasses = compClasses
    end,
    Matcher
)

function MulComponentMatcher:IsMatch(entityObject)
    oop.error("MulComponentMatcher","This is abstract class")

    return false
end

return MulComponentMatcher
