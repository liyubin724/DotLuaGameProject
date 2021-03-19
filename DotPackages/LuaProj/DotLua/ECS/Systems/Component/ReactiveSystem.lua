local oop = require('DotLua/OOP/oop')
local ExecuteSystem = oop.using('Dotlua/ECS/Systems/Component/ExecuteSystem')
local Collector = oop.using('DotLua/ECS/Collectors/Collector')

local ReactiveSystem =
    oop.class(
    'ReactiveSystem',
    function(self, group, frameInterval, groupEvents)
        self.collector = Collector(group, groupEvents)
    end,
    ExecuteSystem
)

function ReactiveSystem:Activate()
    self.collector:Activate()
end

function ReactiveSystem:Deactivate()
    self.collector:Deactivate()
end

function ReactiveSystem:Execute()
    local entities = self.collector:GetCollectedEntities()

    if entities and #entities > 0 then
        for _, entity in ipairs(entities) do
            self:OnEntityUpdate(entity)
        end
    end

    self.collector:ClearCollectedEntities()
end

function ReactiveSystem:OnEntityUpdate(entity)
    oop.error('ECS', 'ReactiveSystem:OnEntityUpdate->this is a abstract class')
end

return ReactiveSystem
