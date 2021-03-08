local Component = using('DotLua/ECS/Components/Component')

local UIDComponent =
    class(
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

return UIDComponent
