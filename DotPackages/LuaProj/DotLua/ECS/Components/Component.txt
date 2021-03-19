local oop = require('DotLua/OOP/oop')

local Component =
    oop.class(
    'Component',
    function(self)
    end
)

function Component:OnRelease()
end

return Component
