local oop = require('DotLua/OOP/oop')

local tcontainsvalue = table.containsvalue
local tinsert = table.insert
local tindexof = table.indexof
local tremove = talbe.remove

local Group =
    oop.class(
    'Group',
    function(self, matcher)
        self.matcher = matcher

        self.entities = {}

        self.onEntityAdded = oop.event()
        self.onEntityRemoved = oop.event()
        self.onEntityUpdated = oop.event()
    end
)

function Group:GetMatcher()
    return self.matcher
end

function Group:GetEntityAddedEvent()
    return self.onEntityAdded
end

function Group:GetEntityRemovedEvent()
    return self.onEntityRemoved
end

function Group:GetEntityUpdatedEvent()
    return self.onEntityUpdated
end

function Group:GetEntityCount()
    return #(self.entities)
end

function Group:HasEntity(entity)
    return tcontainsvalue(self.entities, entity)
end

function Group:AddEntity(entity)
    if self.matcher:IsMatch(entity) then
        tinsert(self.entities, entity)

        self.onEntityAdded:Invoke(self, entity)
    end
end

function Group:RemoveEntity(entity)
    if self:HasEntity(entity) then
        local index = tindexof(self.entities, entity)

        tremove(self.entities, index)

        self.onEntityRemoved:Invoke(self, entity)
    end
end

function Group:UpdateEntity(entity,oldComponent,newComponent)
    local isMatch = self.matcher:IsMatch(entity)
    local hasEntity = self:HasEntity(entity)
    if isMatch and hasEntity then

    elseif isMatch and not hasEntity then

    elseif not isMatch and hasEntity then
        
    end
end

return Group
