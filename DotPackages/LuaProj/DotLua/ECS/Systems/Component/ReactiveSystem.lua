local oop = require('DotLua/OOP/oop')
local ExecuteSystem = oop.using('Dotlua/ECS/Systems/Component/ExecuteSystem')

local ReactiveSystem =
    oop.class(
    'ReactiveSystem',
    function(self)
    end,
    ExecuteSystem
)

function ReactiveSystem:Filter(entity)
    oop.error("ReactiveSystem","")
    return false
end

function ReactiveSystem:DoExecute(deltaTime,unscaleDeltaTime)

end

return ReactiveSystem
