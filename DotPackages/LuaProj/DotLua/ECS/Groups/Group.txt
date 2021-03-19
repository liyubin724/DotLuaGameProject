local oop = require('DotLua/OOP/oop')
local GroupEvent = oop.using('DotLua/ECS/Groups/GroupEvent')

local tlength = table.length
local tvalues = table.values

local Group =
    oop.class(
    'Group',
    function(self)
        self.matcher = nil

        self.entitiesCache = nil
        self.entityDic = {}

        self.onEntityAddedEvent = oop.event()
        self.onEntityRemovedEvent = oop.event()
        self.onEntityModifiedEvent = oop.event()

        self.refCount = 0
    end
)

function Group:GetMatcher()
    return self.matcher
end

function Group:SetMatcher(matcher)
    self.matcher = matcher
end

function Group:GetEntityCount()
    return tlength(self.entityDic)
end

function Group:BindEvent(receiver, addedFunc, removedFunc, modifiedFunc)
    if addedFunc then
        self.onEntityAddedEvent:Add(receiver, addedFunc)
    end

    if removedFunc then
        self.onEntityRemovedEvent:Add(receiver, removedFunc)
    end

    if modifiedFunc then
        self.onEntityModifiedEvent:Add(receiver, modifiedFunc)
    end
end

function Group:UnbindEvent(receiver, addedFunc, removedFunc, modifiedFunc)
    if addedFunc then
        self.onEntityAddedEvent:Remove(receiver, addedFunc)
    end

    if removedFunc then
        self.onEntityRemovedEvent:Remove(receiver, removedFunc)
    end

    if modifiedFunc then
        self.onEntityModifiedEvent:Remove(receiver, modifiedFunc)
    end
end

function Group:GetEntities()
    if self.entitiesCache then
        self.entitiesCache = tvalues(self.entityDic)
    end

    return self.entitiesCache
end

function Group:GetRefCount()
    return self.refCount
end

function Group:RetainRef()
    self.refCount = self.refCount + 1
end

function Group:ReleaseRef()
    self.refCount = self.refCount - 1
end

function Group:TryAddEntity(entity, eventType, param1, param2)
    local uid = entity:GetUID()
    if not self.entityDic[uid] then
        self.entitiesCache = nil
        self.entityDic[uid] = entity

        self.onEntityAddedEvent:Invoke(entity, eventType, param1, param2)
    end
end

function Group:TryRemoveEntity(entity, eventType, param1, param2)
    local uid = entity:GetUID()
    if self.entityDic[uid] then
        self.entitiesCache = nil
        self.entityDic[uid] = nil

        self.onEntityRemovedEvent:Invoke(entity, eventType, param1, param2)
    end
end

function Group:ModifyEntity(entity, eventType, param1, param2)
    local uid = entity:GetUID()
    if self.entityDic[uid] then
        self.onEntityModifiedEvent:Invoke(entity, eventType, param1, param2)
    end
end

return Group
