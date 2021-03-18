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
        self.entities = {}

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
        self.entitiesCache = tvalues(self.entities)
    end

    return self.entitiesCache
end

function Context:HasEntity(entity)
    return tcontainsvalue(self.entities, entity)
end

function Context:HasEntityByUID(uid)
    return self.entities[uid] ~= nil
end

function Context:GetEntityByUID(uid)
    return self.entities[uid]
end

function Context:GetEntityByUIDs(uids)
    local result = {}
    for _, uid in ipairs(uids) do
        local entity = self.entities[uid]
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
    entity:SetData(self, uid)

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

    self.entitiesCache = nil
    self.entities[uid] = entity

    entity:BindEvent(EntityEventType.ComponentAdded, self.onEntityComponentAdded)
    entity:BindEvent(EntityEventType.ComponentRemoved, self.onEntityComponentRemoved)
    entity:BindEvent(EntityEventType.ComponentReplaced, self.onEntityComponentReplaced)
    entity:BindEvent(EntityEventType.ComponentModified, self.onEntityComponentModified)

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
        self.entities[uid] = nil

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

function Context:GetGroup(matcher)
    local group = self.groupDic[matcher]
    if group then
        return group
    end

    group = self.groupPool:Get()
    --TODO:对于新创建的Group，进行分类Entity如何处理，是否需要通知Group的变化
    -- for _, entity in ipairs(self.entities) do
    --     if matcher:IsMatch(entity) then
    --         group:AddEntity(entity)
    --     end
    -- end

    self.groupDic[matcher] = group
    return group
end

function Context:onEntityCreated(entity)
end

function Context:onEntityReleased(entity)
end

function Context:onEntityComponentAdded(entity, addedComponent)
end

function Context:onEntityComponentRemoved(entity, removedComponent)
end

function Context:onEntityComponentReplaced(entity, oldComponent, newComponent)
end

function Context:onEntityComponentModified(entity, modifyComponent, modifyTag)
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
