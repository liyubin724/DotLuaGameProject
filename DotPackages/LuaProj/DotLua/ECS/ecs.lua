local oop = require('DotLua/OOP/oop')
local MessageDispatcher = oop.using('DotLua/Message/MessageDispatcher')

local ecs = {}
ecs.dispatcher = MessageDispatcher()

return ecs