local oop = require('DotLua/OOP/oop')

local Matcher =
    oop.class(
    'Matcher',
    function(self)
    end
)

function Matcher:IsMatch(entityObj)
    return false
end

return Matcher
