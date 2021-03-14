local oop = require('DotLua/OOP/oop')

local GroupEvent = oop.enum('GroupEvent', {Added = 1, Removed = 2, AddedOrRemoved = 3})
return GroupEvent
