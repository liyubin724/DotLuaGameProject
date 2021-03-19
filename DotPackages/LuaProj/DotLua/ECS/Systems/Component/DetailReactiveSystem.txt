local oop = require('DotLua/OOP/oop')
local ExecuteSystem = oop.using('Dotlua/ECS/Systems/Component/ExecuteSystem')
local DetailCollector = oop.using('DotLua/ECS/Collectors/DetailCollector')

local DetailReactiveSystem =
    oop.class(
    'DetailReactiveSystem',
    function(self, group, frameInterval, groupEvents)
        self.detailCollector = DetailCollector(group, groupEvents)
    end,
    ExecuteSystem
)

function DetailReactiveSystem:DoInitialize()
    self.detailCollector:Activate()
end

function DetailReactiveSystem:DoTeardown()
    self.detailCollector:Deactivate()
end

function DetailReactiveSystem:Execute()
    local addedEntities = self.detailCollector:GetAddedEntities()
    if addedEntities and #(addedEntities) > 0 then
        for _, data in ipairs(addedEntities) do
            self:OnEntityAdded(data)
        end
    end

    local removedEntities = self.detailCollector:GetAddedEntities()
    if removedEntities and #(removedEntities) > 0 then
        for _, data in ipairs(removedEntities) do
            self:OnEntityRemoved(data)
        end
    end

    local modifiedEntities = self.detailCollector:GetAddedEntities()
    if modifiedEntities and #(modifiedEntities) > 0 then
        for _, data in ipairs(modifiedEntities) do
            self:OnEnityModified(data)
        end
    end
end

function DetailReactiveSystem:OnEntityAdded(detailEntityData)
    oop.error('ECS', 'DetailReactiveSystem:OnEntityAdded->this is a abstract class')
end

function DetailReactiveSystem:OnEntityRemoved(detailEntityData)
    oop.error('ECS', 'DetailReactiveSystem:OnEntityRemoved->this is a abstract class')
end

function DetailReactiveSystem:OnEnityModified(detailEntityData)
    oop.error('ECS', 'DetailReactiveSystem:OnEnityModified->this is a abstract class')
end

return DetailReactiveSystem
