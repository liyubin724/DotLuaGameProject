local oop = require('DotLua/OOP/oop')
local DebugLogger = oop.using('DotLua/Log/DebugLogger')
local Entity = oop.using("DotLua/ECS/Entities/Entity")

local ObjectPool = using("DotLua/Pool/ObjectPool")
local List = using('DotLua/Generic/Collections/List')

local Context =
    class(
    'Context',
    function(self,name)
        self.name = name

        self.onEntityCreated = Event()
        self.onEntityWillBeDestroyed = Event()
        self.onEntityDestroyed = Event()
        self.onEntityGroupCreated = Event()

        self.allEntities = {}

        self.entityPool = ObjectPool(Entity)
        self.componentPoolList = List()
    end
)

function Context:GetName()
    return self.name
end

function Context:CreateEntity()

end

return Context