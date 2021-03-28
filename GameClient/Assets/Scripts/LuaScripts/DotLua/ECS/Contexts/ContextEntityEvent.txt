local oop = require('DotLua/OOP/oop')

local ContextEntityEvent =
    oop.enum(
    'ContextEntityEvent',
    {
        EntityCreated = 1,
        EntityReleased = 2,
        ComponentAdded = 1,
        ComponentRemoved = 2,
        ComponentReplaced = 3,
        ComponentModified = 4,
        GroupCreated = 5
    }
)

return ContextEntityEvent
