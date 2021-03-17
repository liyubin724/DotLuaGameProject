local oop = require('DotLua/OOP/oop')
local EntityEventType = oop.using('DotLua/ECS/Entities/EntityEventType')

local tlength = table.length
local tvalues = table.values
local tkeys = table.keys

local Entity =
    oop.class(
    'Entity',
    function(self)
        self.enable = true
        self.context = nil

        self.uid = -1

        self.cachedComponents = nil
        self.componentDic = {}

        --params:EntityEventType,preComponent,newComponent
        self.onComponentEvent = oop.event()
    end
)

function Entity:GetEnable()
    return self.enable
end

function Entity:GetContext()
    return self.context
end

function Entity:GetUID()
    return self.uid
end

function Entity:SetData(context,uid)
    self.context = context
    self.uid = uid
end

function Entity:GetComponentEvent()
    return self.onComponentEvent
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
    if oop.isDebug then
        if not oop.isclass(componentClass) then
            oop.error('ECS', 'Entity:HasComponent->the param is not a class')
            return false
        end
    end

    local component = self.componentDic[componentClass]
    if not component then
        for k, _ in pairs(self.componentDic) do
            if oop.iskindof(k, componentClass) then
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
            if oop.iskindof(k, componentClass) then
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
        oop.error('ECS', 'Entity:AddComponent->The enity has been disabled')
        return
    end
    if oop.isDebug then
        if self:HasComponent(componentClass) then
            oop.error(
                'ECS',
                string.format(
                    'Entity:AddComponent->The component of %s has beend added!',
                    componentClass:GetClassName()
                )
            )
            return nil
        end
    end

    self.cachedComponents = nil
    local component = self:addComp(componentClass)
    self.onComponentEvent:Invoke(self, EntityEventType.ComponentAdded, component, nil)

    return component
end

function Entity:addComp(componentClass)
    local component = self.context:createComponent(componentClass)
    self.componentDic[componentClass] = component

    return component
end

function Entity:RemoveComponent(componentClass)
    if not self.enable then
        oop.error('ECS', 'Entity:RemoveComponent->The enity has been disabled')
        return
    end

    self.cachedComponents = nil
    local component = self:removeComp(componentClass)
    if component then
        self.onComponentEvent:Invoke(self, EntityEventType.ComponentRemoved, component, nil)
        self.context:releaseComponent(component)
    end
end

function Entity:removeComp(componentClass)
    local key = componentClass
    local component = self.componentDic[key]
    if not component then
        for k, v in pairs(self.componentDic) do
            if oop.iskindof(k, componentClass) then
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
        oop.error('ECS', 'Entity:ReplaceComponent->The enity has been disabled')
        return
    end

    self.cachedComponents = nil

    local oldComponent = self:removeComp(oldComponentClass)
    if oldComponent then
        if newComponentClass then
            local newComponent = self:addComp(newComponentClass)
            if newComponentClass then
                self.onComponentEvent:Invoke(self, EntityEventType.ComponentReplaced, oldComponent, newComponent)
            else
                oop.error('ECS', 'Entity:ReplaceComponent->Added Failed')
            end
        else
            self.onComponentEvent:Invoke(self, EntityEventType.ComponentRemoved, oldComponent, nil)
        end

        self.context:releaseComponent(oldComponent)
    else
        if newComponentClass then
            local newComponent = self:addComp(newComponentClass)
            if newComponent then
                self.onComponentEvent:Invoke(self, EntityEventType.ComponentAdded, newComponent, nil)
            else
                oop.error('ECS', 'Entity:ReplaceComponent->Added Failed')
            end
        end
    end
end

function Entity:ModifyComponent(componentClass, modifyDelegate)
    if not self.enable then
        oop.error('ECS', 'Entity:ModifyComponent->The enity has been disabled')
        return
    end

    local component = self:GetComponent(componentClass)
    if not component then
        modifyDelegate:ActionInvoke(self, component)
        self.onComponentEvent:Invoke(self, EntityEventType.ComponentModified, component, nil)
    else
        oop.error('ECS', 'Entity:ModifyComponent->The component is not found')
    end
end

function Entity:MarkComponentModified(component)
    if not self.enable then
        oop.error('ECS', 'Entity:ModifyComponent->The enity has been disabled')
        return
    end
    if component then
        self.onComponentEvent:Invoke(self, EntityEventType.ComponentModified, component, nil)
    end
end

function Entity:removeAllComponents()
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
    self.uid = -1
    self.context = nil
    self.cachedComponents = nil
    self:removeAllComponents()

    self.onComponentEvent:Clear()
end

function Entity:Destroy()
    self.context:ReleaseEntity(self)
end

return Entity
