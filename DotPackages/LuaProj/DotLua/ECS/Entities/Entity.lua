local oop = require('DotLua/OOP/oop')

local tlength = table.length
local tvalues = table.values
local tkeys = table.keys

local Entity =
    oop.class(
    'Entity',
    function(self)
        self.enable = true
        self.context = nil

        self.cachedComponents = nil
        self.componentDic = {}

        self.onComponentAddedEvent = oop.event()
        self.onComponentRemovedEvent = oop.event()
        self.onComponentReplacedEvent = oop.event()
    end
)

function Entity:GetEnable()
    return self.enable
end

function Entity:GetContext()
    return self.context
end

function Entity:SetContext(context)
    self.context = context
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

function Entity:GetComponentCount()
    return tlength(self.componentDic)
end

function Entity:GetAllComponents()
    if not self.cachedComponents then
        self.cachedComponents = tvalues(self.componentDic)
    end
    return self.cachedComponents
end

function Entity:HasComponent(componentClass)
    local component = self.componentDic[componentClass]
    if not component then
        for k, _ in pairs(self.componentDic) do
            if k:IsKindOf(componentClass) then
                return true
            end
        end

        return false
    else
        return true
    end
end

function Entity:HasComponents(componentClasses)
    for _, component in ipairs(componentClasses) do
        if not self:HasComponent(component) then
            return false
        end
    end

    return true
end

function Entity:HasAnyComponent(componentClasses)
    for _, component in ipairs(componentClasses) do
        if self:HasComponent(component) then
            return true
        end
    end

    return false
end

function Entity:GetComponent(componentClass)
    local component = self.componentDic[componentClass]
    if not component then
        for k, v in pairs(self.componentDic) do
            if k:IsKindOf(componentClass) then
                return v
            end
        end

        return nil
    else
        return component
    end
end

function Entity:AddComponent(componentClass)
    if not self.enable then
        oop.Logger.Error('Entity', 'The enity has been disabled')
        return
    end

    if self:HasComponent(componentClass) then
        oop.Logger.Error('Entity', string.format('The component of %s has beend added!', componentClass:GetClassName()))
        return
    end

    self.cachedComponents = nil

    local component = self:addComp(componentClass)
    self.onComponentAddedEvent:Invoke(self, component)

    return component
end

function Entity:addComp(componentClass)
    local component = self.context:createComponent(componentClass)
    self.componentDic[componentClass] = component

    return component
end

function Entity:RemoveComponent(componentClass)
    if not self.enable then
        oop.Logger.Error('Entity', 'The enity has been disabled')
        return
    end

    self.cachedComponents = nil

    local component = self:removeComp(componentClass)
    if component then
        self.onComponentRemovedEvent:Invoke(self, component)

        self.context:releaseComponent(component)
    end
end

function Entity:removeComp(componentClass)
    local key = componentClass

    local component = self.componentDic[key]
    if not component then
        for k, v in pairs(self.componentDic) do
            if k:IsKindOf(componentClass) then
                key = k
                component = v
                break
            end
        end
    end

    if not component then
        self.componentDic[key] = nil
    end
    return component
end

function Entity:ReplaceComponent(oldComponentClass, newComponentClass)
    if not self.enable then
        oop.Logger.Error('Entity', 'The enity has been disabled')
        return
    end

    self.cachedComponents = nil

    local oldComponent = self:removeComp(oldComponentClass)
    if oldComponent then
        if newComponentClass then
            local newComponent = self:addComp(newComponentClass)
            self.onComponentReplacedEvent:Invoke(oldComponent, newComponent)
        else
            self.onComponentRemovedEvent:Invoke(oldComponent)
        end

        self.context:releaseComponent(oldComponent)
    else
        if newComponentClass then
            local newComponent = self:addComp(newComponentClass)
            self.onComponentAddedEvent:Invoke(newComponent)
        end
    end
end

function Entity:RemoveAllComponents()
    local keys = tkeys(self.componentDic)
    for i = 1, #(keys), 1 do
        local component = self.componentDic[keys[i]]
        self.componentDic[keys[i]] = nil

        self.context:releaseComponent(component)
    end
end

function Entity:OnGet()
    self.enable = true
end

function Entity:OnRelease()
    self.enable = false
    self.cachedComponents = nil
    self:RemoveAllComponents()

    self.onComponentAddedEvent:Clear()
    self.onComponentRemovedEvent:Clear()
    self.onComponentReplacedEvent:Clear()
end

function Entity:Destroy()
    self.context:ReleaseEntity(self)
end

return Entity
