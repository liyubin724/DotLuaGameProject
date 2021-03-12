local oop = require('DotLua/OOP/oop')

local Group =
    oop.class(
    'Group',
    function(self,matcher)
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

function Group:GetEntityCount()
    return #(self.entities)
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

function Group:Count()
    return #(self.entityList)
end

return Group
