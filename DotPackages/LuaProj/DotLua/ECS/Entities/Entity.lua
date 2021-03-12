local oop = require('DotLua/OOP/oop')

local tinsert = table.insert
local tremove = table.remove
local tcopyto = table.copyto

local Entity =
    oop.class(
    'Entity',
    function(self)
        self.isEnable = true

        self.cachedComponents = {}
        self.components = {}

        self.onComponentAddedEvent = oop.event()
        self.onComponentRemovedEvent = oop.event()
        self.onComponentReplacedEvent = oop.event()
    end
)

function Entity:GetIsEnable()
    return self.isEnable
end

function Entity:SetIsEnable(isEnable)
    self.isEnable = isEnable
end

function Entity:GetComponentAddedEvent()
    return self.onComponentAddedEvent
end

function Entity:GetComponentRemovedEvent()
    return self.onComponentRemovedEvent
end

function Entity:GetComponentReplacedEvent()
    return self.onComponentReplacedEvent
end

function Entity:GetComponentTotalCount()
    return #(self.components)
end

function Entity:GetAllComponents()
    if not self.cachedComponents then
        tcopyto(self.components, self.cachedComponents)
    end
    return self.cachedComponents
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
        oop.Logger.Error('Entity', 'The enity has been disabled')
        return
    end

    if self:HasComponent(componentClass) then
        oop.Logger.Error('Entity', string.format('The component of %s has beend added!', componentClass:GetClassName()))
        return
    end

    local component = self.context:CreateComponent(componentClass)
    tinsert(self.components, component)

    self.cachedComponents = nil

    self.onComponentAddedEvent:Invoke(self, component)

    return component
end

function Entity:RemoveComponent(componentClass)
    if not self.isEnable then
        oop.Logger.Error('Entity', 'The enity has been disabled')
        return
    end

    local index,component = self:getComponentAndIndex(componentClass)
    if index > 0 then
        tremove(self.components,index)
        self.onComponentRemovedEvent:Invoke(self,component)
    end
end

function Entity:ReplaceComponent(oldComponentClass, newComponentClass)
    if not self.isEnable then
        oop.Logger.Error('Entity', 'The enity has been disabled')
        return
    end

    local index, oldComponent = self:getComponentAndIndex(oldComponentClass)
    if index > 0 then
        if not newComponentClass then
            tremove(self.components, index)
            self.onComponentRemovedEvent:Invoke(self, oldComponent)
        else

        end
    else
        if newComponentClass then
            local component = self.context:CreateComponent(newComponentClass)
            tinsert(self.components, component)

            self.cachedComponents = nil

            self.onComponentAddedEvent:Invoke(self, component)
        end
    end


    if oldComponentClass == newComponentClass then
        return
    end



    local oldComponent = self:removeComp(oldComponentClass)
    if not oldComponent and newComponentClass then
        self:AddComponent(newComponentClass)
    elseif oldComponent then
        if newComponentClass then
        else
        end
    end

    if self:HasComponent(oldComponentClass) then
        local oldComponent = self:removeComp(componentClass)

        for index, component in ipairs(self.components) do
            if component:IsKindOf(oldComponentClass) then
                oldComponent = component
                tremove(self.components, index)
                break
            end
        end

        if newComponentClass then
            local newComponent = context:GetComponent(newComponentClass)
            self:AddComponentInstance(newComponent)

            if self.onComponentReplaced then
                self.onComponentReplaced(self, oldComponent, newComponent)
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

function Entity:getComponentIndex(componentClass)
    for index, component in ipairs(self.components) do
        if component:IsKindOf(componentClass) then
            return index
        end
    end
    return -1
end

return Entity
