local oop = require('DotLua/OOP/oop')
local GroupEvent = oop.using('DotLua/ECS/Groups/GroupEvent')
local ObjectPool = oop.using('DotLua/Pool/ObjectPool')
local DetailEntityData = oop.using("DotLua/ECS/Collectors/DetailEntityData")

local tinsert = table.insert
local tremove = table.remove

local DetailCollector =
    oop.class(
    'DetailCollector',
    function(self, group, groupEvents)
        self.addedEntities = {}
        self.removedEntities = {}
        self.modifiedEntities = {}

        self.group = group
        self.groupEvents = groupEvents

        self.detailEntityDataPool = ObjectPool(DetailEntityData)
    end
)

function DetailCollector:GetAddedEntityCount()
    return #(self.addedEntities)
end

function DetailCollector:GetRemovedEntityCount()
    return #(self.removedEntities)
end

function DetailCollector:GetModifyEntityCount()
    return #(self.modifiedEntities)
end

function DetailCollector:GetAddedEntities()
    return self.addedEntities
end

function DetailCollector:GetRemovedEntities()
    return self.removedEntities
end

function DetailCollector:GetModifiedEntities()
    return self.modifiedEntities
end

function DetailCollector:ClearAddedEntities()
    for i = #(self.addedEntities), 1, -1 do
        self.detailEntityDataPool:Release(self.addedEntities[i])
        tremove(self.addedEntities,i)
    end
end

function DetailCollector:ClearRemovedEntities()
    for i = #(self.removedEntities), 1, -1 do
        self.detailEntityDataPool:Release(self.removedEntities[i])
        tremove(self.removedEntities, i)
    end
end

function DetailCollector:ClearModifiedEntities()
    for i = #(self.modifiedEntities), 1, -1 do
        self.detailEntityDataPool:Release(self.modifiedEntities[i])
        tremove(self.modifiedEntities, i)
    end
end

function DetailCollector:ClearAllEntities()
    self:ClearAddedEntities()
    self:ClearRemovedEntities()
    self.ClearModifiedEntities()
end

function DetailCollector:Activate()
    if self.groupEvents and #self.groupEvents > 0 and self.group then
        local addedFunc = nil
        local removedFunc = nil
        local modifiedFunc = nil

        for _, eventType in ipairs(self.groupEvents) do
            if eventType == GroupEvent.EntityRemoved then
                addedFunc = self.onGroupChanged
            elseif eventType == GroupEvent.EntityAdded then
                removedFunc = self.onGroupChanged
            elseif eventType == GroupEvent.EntityModified then
                modifiedFunc = self.onGroupChanged
            end
        end

        self.group:BindEvent(self, addedFunc, removedFunc, modifiedFunc)
    end
end

function DetailCollector:Deactivate()
    if self.groupEvents and #self.groupEvents > 0 and self.group then
        local addedFunc = nil
        local removedFunc = nil
        local modifiedFunc = nil

        for _, eventType in ipairs(self.groupEvents) do
            if eventType == GroupEvent.EntityRemoved then
                addedFunc = self.onEntityRemoved
            elseif eventType == GroupEvent.EntityAdded then
                removedFunc = self.onEntityAdded
            elseif eventType == GroupEvent.EntityModified then
                modifiedFunc = self.onEntityModified
            end
        end

        self.group:UnbindEvent(self, addedFunc, removedFunc, modifiedFunc)
    end
end

function DetailCollector:onEntityAdded(entity, eventType, param1, param2)
    local data = self.detailEntityDataPool:Get()
    data:Init(entity,eventType,param1,param2)

    tinsert(self.addedEntities,data)
end

function DetailCollector:onEntityRemoved(entity, eventType, param1, param2)
    local data = self.detailEntityDataPool:Get()
    data:Init(entity, eventType, param1, param2)

    tinsert(self.removedEntities, data)
end

function DetailCollector:onEntityModified(entity, eventType, param1, param2)
    local data = self.detailEntityDataPool:Get()
    data:Init(entity, eventType, param1, param2)

    tinsert(self.modifiedEntities, data)
end

return DetailCollector
