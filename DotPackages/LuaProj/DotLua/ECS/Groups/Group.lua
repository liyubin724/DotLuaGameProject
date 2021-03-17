local oop = require('DotLua/OOP/oop')
local GroupEventType = oop.using('DotLua/ECS/Groups/GroupEventType')

local tlength = table.length

local Group =
    oop.class(
    'Group',
    function(self)
        self.entities = {}

        self.onEntityEvent = oop.event()
    end
)

function Group:GetEntityEvent()
    return self.onEntityEvent
end

function Group:GetEntityCount()
    return tlength(self.entities)
end

function Group:HasEntity(entity)
    local uid = entity:GetUID()
    return self.entities[uid] ~= nil
end

function Group:TryAddEntity(entity)
    local uid = entity:GetUID()
    if not self.entities[uid] then
        self.entities[uid] = entity

        self.onEntityEvent:Invoke(GroupEventType.EntityAdded, entity)
    end
end

function Group:TryRemoveEntity(entity)
    local uid = entity:GetUID()
    if self.entities[uid] then
        self.entities[uid] = nil

        self.onEntityEvent:Invoke(GroupEventType.EntityRemoved, entity)
    end
end

function Group:ModifyEntity(entity)
    if self:HasEntity(entity) then
        self.onEntityEvent:Invoke(GroupEventType.EntityModified, entity)
    end
end

return Group
