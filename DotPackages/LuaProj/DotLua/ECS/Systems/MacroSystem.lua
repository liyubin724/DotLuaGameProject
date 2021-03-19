local oop = require('DotLua/OOP/oop')
local System = oop.using("DotLua/ECS/Systems/System")

local MacroSystem =
    oop.class(
    'System',
    function(self,...)

    end,
    System
)

function MacroSystem:Add(system)

end

function MacroSystem:Remove(system)

end

function MacroSystem:DoInitialize()

end

return MacroSystem
