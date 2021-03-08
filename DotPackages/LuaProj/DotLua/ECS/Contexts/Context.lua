local Event = using("DotLua/Core/Event")
local ObjectPool = using("DotLua/Pool/ObjectPool")
local Entity = using("DotLua/ECS/Entities/Entity")
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




return EntityContext