local oop = require('DotLua/OOP/oop')
local UIDComponent = oop.using("DotLua/ECS/Components/UIDComponent")
local Entity = oop.using("DotLua/ECS/Entities/Entity")

local ObjectPool = oop.using("DotLua/Pool/ObjectPool")

local Context =
    oop.class(
    'Context',
    function(self,name,uidCreator)
        self.name = name
        self.uidCreator = uidCreator

        self.componentClassPools = {}
        self.entityPool = ObjectPool(Entity)
    end
)

function Context:GetName()
    return self.name
end

function Context:GetEntity()
    local entity = self.entityPool:Get()
    local uidComp = self:GetComponent(UIDComponent)
    uidComp:SetUID(self.uidCreator:GetNextUID())
    entity:AddComponentInstance(uidComp)

    return entity
end

function Context:ReleaseEntity(entity)
    self.entityPool:Release(entity)
end

function Context:GetComponent(componentClass)
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

function Context:GetGroup(matcher)
    
end

function Context:Destroy()

end

return Context