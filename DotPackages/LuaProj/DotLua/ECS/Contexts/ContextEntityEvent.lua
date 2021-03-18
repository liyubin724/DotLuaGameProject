local oop = require('DotLua/OOP/oop')

local ContextEntityEvent =
    oop.enum(
    'ContextEntityEvent',
    {
        Created = 1,
        Released = 2,
        ComponentAdded = 1,
        ComponentRemoved = 2,
        ComponentReplaced = 3,
        ComponentModified = 4
    }
)

return ContextEntityEvent
