local oop = require('DotLua/OOP/oop')

local Matcher =
    oop.class(
    'Matcher',
    function(self,name)
        self.name = name
    end
)

function Matcher:IsMatch(entityObj)
    oop.error("Matcher","This is abstract class")
    return false
end

return Matcher
