local oop = require('DotLua/OOP/oop')
local Component = require('DotLua/ECS/Components/Component')
local Group = require("DotLua/ECS/Groups/Group")
local UIDComponent = oop.using('DotLua/ECS/Components/UIDComponent')
local Entity = oop.using('DotLua/ECS/Entities/Entity')
local ObjectPool = oop.using('DotLua/Pool/ObjectPool')

local select = select
local tinsert = table.insert
local tcontainsvalue = table.containsvalue

local Context =
    oop.class(
    'Context',
    function(self, name, uidCreator)
        self.name = name
        self.uidCreator = uidCreator

        self.componentClassPools = {}
        self.entityPool = ObjectPool(Entity)

        self.entities = {}
        self.groupDic = {}

        self.onGroupCreated = oop.event()
    end
)

function Context:GetName()
    return self.name
end

function Context:CreateEntity(...)
    local entity = self.entityPool:Get()
    entity:SetEnable(true)
    entity:SetContext(self)

    local addEvent = entity:GetComponentAddedEvent()
    local removeEvent = entity:GetComponentRemovedEvent()
    local replaceEvent = entity:GetComponentReplacedEvent()

    addEvent:Add(self, self.onEntityComponentAdded)
    removeEvent:Add(self, self.onEntityComponentRemoved)
    replaceEvent:Add(self, self.onEntityComponentReplace)

    local uidComp = entity:AddComponent(UIDComponent)
    uidComp:SetUID(self.uidCreator:GetNextUID())

    if select('#', ...) > 0 then
        for i = 1, select('#', ...), 1 do
            local componentClass = select(i, ...)
            if not componentClass or not componentClass.IsKindOf or not componentClass:IsKindOf(Component) then
                oop.error('Context', 'the class is not a subclass of Component')
                self:ReleaseEntity(entity)

                return nil
            end
            entity:AddComponent(componentClass)
        end
    end

    tinsert(self.entities, entity)
    return entity
end

function Context:ReleaseEntity(entity)
    entity:RemoveAllComponents()
    self.entityPool:Release(entity)
end

function Context:HasEntity(entity)
    return tcontainsvalue(self.entities, entity)
end

function Context:ReleaseAllEntities()
end

function Context:GetEntities()
end

function Context:CreateComponent(componentClass)
    local pool = self.componentClassPools[componentClass]
    if not pool then
        pool = ObjectPool(componentClass)
        self.componentClassPools[componentClass] = pool
    end

    return pool:Get()
end

function Context:ReleaseComponent(componentObj)
    local componentClass = componentObj:GetClass()

    local pool = self.componentClassPools[componentClass]
    if pool then
        pool:Release(componentObj)
    end
end

function Context:CreateGroup(matcher)
    local group = self.groupDic[matcher]
    if group then
        return group
    end

    group = Group(matcher)
    self.groupDic[matcher] = group

    for _, entity in ipairs(self.entities) do
        
    end
    return group
end

function Context:Destroy()
end

function Context:onEntityComponentAdded(component)
end

function Context:onEntityComponentRemoved(component)
end

function Context:onEntityComponentReplace(oldComponent, newComponent)
end

return Context
