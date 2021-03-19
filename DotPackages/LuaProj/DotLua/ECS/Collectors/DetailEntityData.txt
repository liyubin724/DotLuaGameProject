local oop = require('DotLua/OOP/oop')
local ContextEntityEvent = oop.using('DotLua/ECS/Contexts/ContextEntityEvent')

local DetailEntityData =
    oop.class(
    'DetailEntityData',
    function(self)
        self.entity = nil
        self.eventType = nil
        self.param1 = nil
        self.param2 = nil
    end
)

function DetailEntityData:GetEntity()
    return self.entity
end

function DetailEntityData:GetEventType()
    return self.eventType
end

function DetailEntityData:GetAddedComponent()
    if self.eventType == ContextEntityEvent.ComponentAdded then
        return self.param1
    end

    return nil
end

function DetailEntityData:GetRemovedComponentClass()
    if self.eventType == ContextEntityEvent.ComponentRemoved then
        return self.param1
    end
    return nil
end

function DetailEntityData:GetReplacedOldComponentClass()
    if self.eventType == ContextEntityEvent.ComponentReplaced then
        return self.param1
    end
    return nil
end

function DetailEntityData:GetReplacedNewComponentClass()
    if self.eventType == ContextEntityEvent.ComponentReplaced then
        return self.param2
    end
    return nil
end

function DetailEntityData:GetModifiedComponent()
    if self.eventType == ContextEntityEvent.ComponentModified then
        return self.param1
    end
    return nil
end

function DetailEntityData:GetModifiedTag()
    if self.eventType == ContextEntityEvent.ComponentModified then
        return self.param2
    end
    return nil
end

function DetailEntityData:Init(entity, eventType, param1, param2)
    self.entity = entity
    self.eventType = eventType
    self.param1 = param1
    self.param2 = param2
end

function DetailEntityData:OnRelease()
    self.entity = nil
    self.eventType = nil
    self.param1 = nil
    self.param2 = nil
end

return DetailEntityData
