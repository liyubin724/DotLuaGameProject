local oop = require('DotLua/OOP/oop')

local tinsert = table.insert
local tcontainsvalue = table.containsvalue
local tsort = table.sort
local tcopy = table.copy
local select = _G.select
local type = _G.type


----public----
-- 使用table表模拟List的实现
-- 1 创建不含任何元素的List
--     local list = List()
-- 2 使用初始化数据初始化List
--     local list = List(1,2,3,4,5,6)
-- 3 获取List的长度
--     local list = List(1,2,3,4,5,6)
-- 3.1 local len = #list
-- 3.2 local len = list:Count()
local List =
    oop.class(
    'List',
    function(self, ...)
        self.list = {}

        for i = 1, select('#', ...) do
            local item = select(i, ...)
            if item then
                tinsert(self.list, item)
            end
        end
    end
)

List.__index = function(list, k)
    if type(k) == 'number' then
        if k >= 1 and k <= #list.list then
            return list.list[k]
        else
            return nil
        end
    end

    return list[k]
end

List.__len = function(self)
    return self:Count()
end

---public----
-- 获取List的长度
-- @return int
function List:Count()
    return #(self.list)
end

----public----
-- 判断List中是否包含有指定的元素
-- @return bool
function List:Contains(item)
    if item then
        return tcontainsvalue(self.list, item)
    end
    return false
end

function List:Get(index)
    if index >= 1 and index <= #(self.list) then
        return self.list[index]
    end

    return nil
end

----public----
-- 向List添加元素
-- 注：无法添加nil到List中
-- @param item 需要添加的元素
-- @return void
function List:Add(item)
    if item then
        tinsert(self.list, item)
    end
end

----public----
-- 向List添加元素
-- 注：无法添加nil到List中
-- @param list 数组形式的table表
-- @return void
function List:AddRange(list)
    if list and type(list) == 'table' and #list > 0 then
        for i = 1, #list do
            self:Add(list[i])
        end
    end
end

function List:Insert(index, item)
    if not item then
        return
    end

    if index < 1 then
        index = 1
    elseif index > #(self.list) then
        index = #(self.list) + 1
    end

    tinsert(self.list, index, item)
end

function List:InsertRange(index, list)
    if list and type(list) == 'table' and #(list) > 0 then
        if index < 1 then
            index = 1
        elseif index > #(self.list) then
            index = #(self.list) + 1
        end
        for i = 1, #list do
            if list[i] then
                self:Insert(index, list[i])
                index = index + 1
            end
        end
    end
end

function List:Remove(item)
    for i = 1, #(self.list) do
        if self.list[i] == item then
            table.remove(self.list, i)
            break
        end
    end
end

function List:RemoveAt(index)
    if index >= 1 and index <= #(self.list) then
        local item = self.list[index]
        table.remove(self.list, index)
        return item
    end

    return nil
end

function List:RemoveRange(list)
    if list and type(list) == 'table' and #(list) > 0 then
        for i = 1, #list do
            self:Remove(list[i])
        end
    end
end

function List:IndexOf(item)
    if not item then
        return -1
    end

    for i = 1, #self.list do
        if self.list[i] == item then
            return i
        end
    end

    return -1
end

function List:LastIndexOf(item)
    if not item then
        return -1
    end

    for i = #(self.list), 1, -1 do
        if self.list[i] == item then
            return i
        end
    end

    return -1
end

function List:Copy()
    local result = List()
    for i = 1, #(self.list) do
        result:Add(self.list[i])
    end
    return result
end

function List:Clear()
    for i = #(self.list), 1, -1 do
        self.list[i] = nil
    end
end

function List:GetEnumerator()
    local index = 0
    local count = #(self.list)

    return function()
        index = index + 1
        if index <= count then
            return self.list[index]
        end
        return nil
    end
end

function List:Find(compareFunc)
    if not compareFunc or type(compareFunc) ~= 'function' then
        error('compareFunc is empty')
        return nil
    end

    for i = 1, #(self.list) do
        local item = self.list[i]
        if compareFunc(item) then
            return item
        end
    end
    return nil
end

function List:ForEach(func)
    for i = 1, #(self.list) do
        func(self.list[i])
    end
end

function List:Sort(compareFunc)
    tsort(self.list, compareFunc)
end

function List:ToTable()
    return tcopy(self.list)
end

return List
