local oop = require('DotLua/OOP/oop')
local Component = require('DotLua/ECS/Components/Component')
local Group = require('DotLua/ECS/Groups/Group')
local Entity = oop.using('DotLua/ECS/Entities/Entity')
local ObjectPool = oop.using('DotLua/Pool/ObjectPool')
local ContextEntityEvent = oop.using('DotLua/ECS/Contexts/ContextEntityEvent')

local tinsert = table.insert
local tcontainsvalue = table.containsvalue
local tvalues = table.values

local Context =
    oop.class(
    'Context',
    function(self, name)
        self.name = name

        self.componentClassPools = {}
        self.entityPool = ObjectPool(Entity)

        self.entitiesCache = nil
        self.entitieDic = {}

        self.groupDic = {}
        self.groupPool = ObjectPool(Group)
        self.onGroupCreated = oop.event()
    end
)

function Context:GetName()
    return self.name
end

function Context:GetEntities()
    if not self.entitiesCache then
        self.entitiesCache = tvalues(self.entitieDic)
    end

    return self.entitiesCache
end

function Context:HasEntity(entity)
    return tcontainsvalue(self.entitieDic, entity)
end

function Context:HasEntityByUID(uid)
    return self.entitieDic[uid] ~= nil
end

function Context:GetEntityByUID(uid)
    return self.entitieDic[uid]
end

function Context:GetEntityByUIDs(uids)
    local result = {}
    for _, uid in ipairs(uids) do
        local entity = self.entitieDic[uid]
        if not entity then
            oop.error('ECS', 'Context:GetEntityByUIDs->the entity of %d is not found')
            return nil
        end
        tinsert(result, entity)
    end
    return result
end

function Context:CreateEntity(uid, componentClasses)
    local entity = self.entityPool:Get()

    if componentClasses and #(componentClasses) > 0 then
        for _, componentClass in ipairs(componentClasses) do
            if oop.isDebug then
                if not oop.isclass(componentClass) or not oop.iskindof(componentClass, Component) then
                    oop.error('ECS', 'Context:CreateEntity->the class is not a subclass of Component')
                    self:ReleaseEntity(entity)
                    return nil
                end
            end
            entity:AddComponent(componentClass)
        end
    end

    entity:Init(self, uid)
    entity:BindEvent(
        self,
        self.onEntityComponentAdded,
        self.onEntityComponentRemoved,
        self.onEntityComponentReplaced,
        self.onEntityComponentModified
    )

    self.entitiesCache = nil
    self.entitieDic[uid] = entity

    self:onEntityCreated(entity)

    return entity
end

function Context:ReleaseEntity(entity)
    local uid = entity:GetUID()
    if uid and uid > 0 then
        self:ReleaseEntityByUID(uid)
    end
end

function Context:ReleaseEntityByUID(uid)
    local entity = self.entities[uid]
    if entity then
        self.entitiesCache = nil
        self.entitieDic[uid] = nil

        self:onEntityReleased(entity)

        self.entityPool:Release(entity)
    end
end

function Context:ReleaseAllEntity()
    -- self.entitiesCache = nil
    -- for i = #self.entities, 1, -1 do
    --     local entity = self.entities[i]
    --     tremove(self.entities, i)
    --     self.entityPool:Release(entity)
    -- end
end

function Context:CreateGroup(matcher)
    local group = self.groupDic[matcher]
    group:RetainRef()

    if group then
        return group
    end

    group = self.groupPool:Get()
    group:SetMatcher(matcher)

    for _, entity in pairs(self.entitieDic) do
        if matcher:IsMatch(entity) then
            group:AddEntity(entity, ContextEntityEvent.GroupCreated)
        end
    end

    self.groupDic[matcher] = group
    return group
end

function Context:ReleaseGroup(group)
end

function Context:onEntityCreated(entity)
    for matcher, group in pairs(self.groupDic) do
        if matcher:IsMatch(entity) then
            group:TryAddEntity(entity, ContextEntityEvent.EntityCreated)
        end
    end
end

function Context:onEntityReleased(entity)
    for matcher, group in pairs(self.groupDic) do
        if matcher:IsMatch(entity) then
            group:TryRemoveEntity(entity, ContextEntityEvent.EntityReleased)
        end
    end
end

function Context:onEntityComponentAdded(entity, addedComponent)
    for matcher, group in pairs(self.groupDic) do
        if matcher:IsMatch(entity) then
            group:TryAddEntity(entity, ContextEntityEvent.ComponentAdded, addedComponent)
        else
            group:TryRemoveEntity(entity, ContextEntityEvent.ComponentAdded, addedComponent)
        end
    end
end

function Context:onEntityComponentRemoved(entity, removedComponent)
    for matcher, group in pairs(self.groupDic) do
        if matcher:IsMatch(entity) then
            group:TryAddEntity(entity, ContextEntityEvent.ComponentRemoved, removedComponent)
        else
            group:TryRemoveEntity(entity, ContextEntityEvent.ComponentRemoved, removedComponent)
        end
    end
end

function Context:onEntityComponentReplaced(entity, oldComponent, newComponent)
    for matcher, group in pairs(self.groupDic) do
        if matcher:IsMatch(entity) then
            group:TryAddEntity(entity, ContextEntityEvent.ComponentReplaced, oldComponent, newComponent)
        else
            group:TryRemoveEntity(entity, ContextEntityEvent.ComponentReplaced, oldComponent, newComponent)
        end
    end
end

function Context:onEntityComponentModified(entity, modifyComponent, modifyTag)
    for matcher, group in pairs(self.groupDic) do
        if matcher:IsMatch(entity) then
            group:TryAddEntity(entity, ContextEntityEvent.ComponentModified, modifyComponent, modifyTag)
        else
            group:TryRemoveEntity(entity, ContextEntityEvent.ComponentModified, modifyComponent, modifyTag)
        end
    end
end

function Context:createComponent(componentClass)
    if oop.isDebug then
        if not oop.isclass(componentClass) or not oop.iskindof(componentClass, Component) then
            oop.error('ECS', 'Context:createComponent->the param is not a class of component')
            return nil
        end
    end
    local pool = self.componentClassPools[componentClass]
    if not pool then
        pool = ObjectPool(componentClass)
        self.componentClassPools[componentClass] = pool
    end

    return pool:Get()
end

function Context:releaseComponent(component)
    local componentClass = component:GetClass()
    local pool = self.componentClassPools[componentClass]
    if pool then
        pool:Release(component)
    end
end

return Context
