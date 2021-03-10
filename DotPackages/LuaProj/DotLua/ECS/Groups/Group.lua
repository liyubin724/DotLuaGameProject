local oop = require('DotLua/OOP/oop')
local List = oop.using('DotLua/Generic/Collections/List')

local Group =
    oop.class(
    'Group',
    function(self,matcher)
        self.matcher = matcher
        self.entityList = List()
    end
)

function Group:GetMatcher()
    return self.matcher
end

function Group:Count()
    return #(self.entityList)
end



return Group
