local oop = require('DotLua/OOP/oop')
local Delegate = oop.using('DotLua/OOP/Delegate')
local DebugLogger = oop.using('DotLua/Log/DebugLogger')

local tinsert = table.insert
local tremove = table.remove
local tlength = table.length
local tcopy = table.copy

local Entity =
    oop.class(
    'Entity',
    function(self, context)
        self.isEnable = true
        self.context = context

        self.components = {}

        self.onComponentAdded = nil
        self.onComponentRemoved = nil
        self.onComponentReplaced = nil
    end
)

function Entity:GetIsEnable()
    return self.isEnable
end

function Entity:SetIsEnable(isEnable)
    self.isEnable = isEnable
end

function Entity:GetContext()
    return self.context
end

function Entity:BindDelegateToAdded(receiver, func)
    self.onComponentAdded = Delegate(receiver, func)
end

function Entity:BindDelegateToRemoved(receiver, func)
    self.onComponentRemoved = Delegate(receiver, func)
end

function Entity:BindDelegateToReplaced(receiver, func)
    self.onComponentReplaced = Delegate(receiver, func)
end

function Entity:GetComponentCount()
    return tlength(self.components)
end

function Entity:GetComponents()
    return tcopy(self.components)
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

function Entity:AddComponent(componentClass)
    if not self.isEnable then
        DebugLogger.Error('Entity', '')
        return
    end

    if self:HasComponent(componentClass) then
        DebugLogger.Error('Entity', '')
        return
    end

    local component = context:GetComponent(componentClass)
    self:AddComponentInstance(component)
end

function Entity:AddComponentInstance(componentInstance)
    tinsert(self.components, componentInstance)

    if self.onComponentAdded then
        self.onComponentAdded:Invoke(self, componentInstance)
    end
end

function Entity:RemoveComponent(componentClass)
    if not self.isEnable then
        DebugLogger.Error('Entity', '')
        return
    end

    local removedComponent = nil
    for index, component in ipairs(self.components) do
        if component:IsKindOf(componentClass) then
            removedComponent = component
            tremove(self.components, index)
            break
        end
    end

    if removedComponent then
        if self.onComponentRemoved then
            self.onComponentRemoved:Invoke(self, removedComponent)
        end

        context:ReleaseComponent(removedComponent)
    end
end

function Entity:ReplaceComponent(oldComponentClass, newComponentClass)
    if not self.isEnable then
        DebugLogger.Error('Entity', '')
        return
    end

    if oldComponentClass == newComponentClass then
        return
    end
    if self:HasComponent(oldComponentClass) then
        if newComponentClass then
            local oldComponent = nil
            for index, component in ipairs(self.components) do
                if component:IsKindOf(oldComponentClass) then
                    oldComponent = component
                    tremove(self.components, index)
                    break
                end
            end

            local newComponent = context:GetComponent(newComponentClass)
            self:AddComponentInstance(newComponent)

            if self.onComponentReplaced then
                self.onComponentReplaced(self,oldComponent,newComponent)
            end

            context:ReleaseComponent(oldComponent)
        else
            self:RemoveComponent(oldComponentClass)
        end
    else
        if newComponentClass then
            self:AddComponent(newComponentClass)
        end
    end
end

function Entity:RemoveAllComponents()
    for i = #(self.components), 1, -1 do
        tremove(self.components, i)
        context:ReleaseComponent(self.components[i])
    end
end

function Entity:Destroy()
    self:RemoveAllComponents()

    context:ReleaseEntity(self)
end

return Entity
