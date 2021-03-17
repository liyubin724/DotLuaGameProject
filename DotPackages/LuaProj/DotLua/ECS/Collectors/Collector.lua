local oop = require('DotLua/OOP/oop')

local tinsert = table.insert
local tcontainsvalue = table.containsvalue
local tclear = table.clear

local Collector =
    oop.class(
    'Collector',
    function(self, group, groupEventType)
        self.collectedEntities = {}
        self.group = group
        self.groupEventType = groupEventType
    end
)

function Collector:GetEntityCount()
    return #self.collectedEntities
end

function Collector:GetCollectedEntities()
    return self.collectedEntities
end

function Collector:ClearCollectedEntities()
    tclear(self.collectedEntities)
end

function Collector:Activate()
    if self.group then
        local groupEvent = self.group:GetEntityEvent()
        groupEvent:Add(self, self.onGroupChanged)
    end
end

function Collector:Deactivate()
    if self.group then
        local groupEvent = self.group:GetEntityEvent()
        groupEvent:Remove(self, self.onGroupChanged)
    end
end

function Collector:onGroupChanged(groupEventType, entity)
    if groupEventType == self.groupEventType and not tcontainsvalue(entity) then
        tinsert(self.collectedEntities, entity)
    end
end

return Collector
