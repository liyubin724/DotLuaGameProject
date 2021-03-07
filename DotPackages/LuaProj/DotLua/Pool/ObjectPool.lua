local remove = _G.table.remove
local insert = _G.table.insert

local ObjectPool =
    class(
    'ObjectPool',
    function(self, itemClass)
        self.itemClass = itemClass
        self.items = {}
    end
)

function ObjectPool:Get()
    local item

    if #(self.items) > 0 then
        item = self.items[1]
        remove(self.items, 1)
    else
        item = self.itemClass()
    end

    if item["OnGet"] ~= nil then
        item:OnGet()
    end

    return item
end

function ObjectPool:Release(item)
    if item then
        if item["OnRelease"] then
            item:OnRelease()
        end

        insert(self.items, item)
    end
end

function ObjectPool:Clear()
    table.clear(self.items)
end

return ObjectPool
