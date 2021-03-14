local oop = require('DotLua/OOP/oop')
local GroupEvent = oop.using('DotLua/ECS/Groups/GroupEvent')

local Collector =
    oop.class(
    'Collector',
    function(self,groups,groupEvents)
        self.collectedEntities = {}

        self.groups = groups
        self.groupEvents = groupEvents
    end
)

function Collector:Active()

end

function Collector:GetEntityCount()
    return #self.collectedEntities
end

return Collector
