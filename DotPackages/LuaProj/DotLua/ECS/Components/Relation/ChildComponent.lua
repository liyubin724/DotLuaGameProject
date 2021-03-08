local Component = using('DotLua/ECS/Components/Component')

local ChildComponent =
    class(
    'ChildComponent',
    function(self)
        self.childUIDs = {}
    end,
    Component
)

function ChildComponent:Has(uid)
    return table.containsvalue(self.childUIDs,uid)
end

function ChildComponent:Add(uid)
    if not self.Has(uid) then
        table.insert(self.childUIDs,uid)
    end
end

function ChildComponent:Remove(uid)
    local index = table.indexof(self.childUIDs, uid)
    if index > 0 then
        table.remove(self.childUIDs,index)
    end
end

function ChildComponent:Count()
    return #self.childUIDs
end


return ChildComponent
