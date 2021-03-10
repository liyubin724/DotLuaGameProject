local oop = require('DotLua/OOP/oop')
local Component = oop.using('DotLua/ECS/Components/Component')

local UIDComponent =
    oop.class(
    'UIDComponent',
    function(self)
        self.uid = -1
    end,
    Component
)

function UIDComponent:GetUID()
    return self.uid
end

function UIDComponent:SetUID(uid)
    self.uid = uid
end

function UIDComponent:OnRelease()
    self.uid = -1
end

return UIDComponent
