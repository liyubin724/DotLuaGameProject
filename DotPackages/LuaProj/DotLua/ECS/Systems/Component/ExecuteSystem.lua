local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local ExecuteSystem =
    oop.class(
    'ExecuteSystem',
    function(self)
    end,
    System
)

function ExecuteSystem:DoExecute(deltaTime,unscaleDeltaTime)
end

return ExecuteSystem
