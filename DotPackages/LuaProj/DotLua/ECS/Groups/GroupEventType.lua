local oop = require('DotLua/OOP/oop')

local GroupEventType =
    oop.enum(
    'GroupEventType',
    {
        EntityAdded = 1,
        EntityRemoved = 2,
        EntityAddedOrRemoved = 3,
        EntityModified = 4,
    }
)
return GroupEventType
