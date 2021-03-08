local List = using('DotLua/Generic/Collections/List')
local isDebug = _G.isDebug or true
local DebugLogger = using('DotLua/Log/DebugLogger')
local Event = using("DotLua/Core/Event")

local Entity =
    class(
    'Entity',
    function(self, context)
        self.isEnable = true
        self.context = context

        self.components = List()

        self.onComponentAdded = Event()
        self.onComponentRemoved = Event()
        self.onComponentReplaced = Event()

        self.onEntityReleased = Event()
        self.onDestroyEntity = Event()
    end
)

function Entity:GetIsEnable()
    return self.isEnable
end

function Entity:SetIsEnable(isEnable)
    self.isEnable = isEnable
end

function Entity:ComponentCount()
    return #self.components
end

function Entity:HasComponent(componentClass)
    for _, component in ipairs(self.components) do
        if component:IsKindOf(componentClass) then
            return true
        end
    end

    return false
end

function Entity:HasComponents(componentClasses)
    for _, component in ipairs(componentClasses) do
        if not self:HasComponent(component) then
            return false
        end
    end

    return true
end

function Entity:HasAnyComponents(componentClasses)
    for _, component in ipairs(componentClasses) do
        if self:HasComponent(component) then
            return true
        end
    end

    return false
end

function Entity:GetComponent(componentClass)
    for _, component in ipairs(self.components) do
        if component:IsKindOf(componentClass) then
            return component
        end
    end
    return nil
end

function Entity:GetComponents()
    return self.components
end

function Entity:AddComponent(componentClass)
    if not self.isEnable then
        DebugLogger.Error('Entity', '')
        return
    end

    if self:HasComponent(componentClass) then
        DebugLogger.Error('Entity', '')
        return
    end

    local component = componentClass()
    self.components:Add(component)

    self.onComponentAdded:Invoke(self,component)
end

function Entity:RemoveComponent(componentClass)
    if not self.isEnable then
        DebugLogger.Error('Entity', '')
        return
    end

    if self:HasComponent(componentClass) then
        DebugLogger.Error('Entity', '')
        return
    end

    self:replaceComp(componentClass, nil)
end

function Entity:ReplaceComponent(oldComponentClass, newComponentClass)
    if not self.isEnable then
        DebugLogger.Error('Entity', '')
        return
    end

    if self:HasComponent(oldComponentClass) then
        self:replaceComp(oldComponentClass, newComponentClass)
    else
        self:AddComponent(newComponentClass)
    end
end

function Entity:RemoveAllComponents()
end


function Entity:replaceComp(oldComponentClass, newComponentClass)

end
return Entity
