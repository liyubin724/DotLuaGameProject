local oop = require('DotLua/OOP/oop')

local GroupEventType =
    oop.enum(
    'GroupEventType',
    {
        EntityAdded = 1,
        EntityRemoved = 2,
        EntityModified = 3
    }
)
return GroupEventType
