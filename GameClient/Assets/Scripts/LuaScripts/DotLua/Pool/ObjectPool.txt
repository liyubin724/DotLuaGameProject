local oop = require('DotLua/OOP/oop')

local tremove = table.remove
local tinsert = table.insert
local tclear = table.clear

local ObjectPool =
    oop.class(
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
        tremove(self.items, 1)
    else
        item = self.itemClass()
    end

    if item['OnGet'] ~= nil then
        item:OnGet()
    end

    return item
end

function ObjectPool:Release(item)
    if item then
        if item['OnRelease'] then
            item:OnRelease()
        end

        tinsert(self.items, item)
    end
end

function ObjectPool:Clear()
    tclear(self.items)
end

return ObjectPool
