local oop = require('DotLua/OOP/oop')

local EntityEventType =
    oop.enum(
    'EntityEventType',
    {
        ComponentAdded = 1,
        ComponentRemoved = 2,
        ComponentReplaced = 3,
        ComponentModified = 4
    }
)

return EntityEventType
