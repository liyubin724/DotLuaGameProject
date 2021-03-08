local Component = using('DotLua/ECS/Components/Component')

local ParentComponent =
    class(
    'ParentComponent',
    function(self)
        self.parentUID = -1
    end,
    Component
)

function ParentComponent:GetParentUID(uid)
    return self.parentUID
end

function ParentComponent:SetParentUID(uid)
    self.parentUID = uid
end

return ParentComponent
