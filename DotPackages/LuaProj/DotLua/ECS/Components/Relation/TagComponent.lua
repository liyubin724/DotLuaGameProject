local Component = using('DotLua/ECS/Components/Component')
local List = using("DotLua/Generic/Collections/List")

local TagComponent =
    class(
    'TagComponent',
    function(self)
        self.tags = List()
    end,
    Component
)

function TagComponent:Has(tag)
    return self.tags:Contains(tag)
end

function TagComponent:Add(tag)
    if not self.tags:Contains(tag) then
        self.tags:Add(tag)
    end
end

function TagComponent:Remove(tag)
    self.tags:Remove(tag)
end

return TagComponent
