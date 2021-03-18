local oop = require('DotLua/OOP/oop')

local GroupEvent =
    oop.enum(
    'GroupEvent',
    {
        EntityAdded = 1,
        EntityRemoved = 2,
        EntityModified = 3
    }
)
return GroupEvent
