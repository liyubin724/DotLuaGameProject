local oop = require('DotLua/OOP/oop')

local CycleTime = oop.using('DotLua/ECS/CycleTime')
local MsgDispatcher = oop.using('DotLua/Message/MsgDispatcher')
local UIDCreator = oop.using('DotLua/Generic/UIDCreator')

local tinsert = table.insert

local ecs = {}

ecs.time = CycleTime()
ecs.dispatcher = MsgDispatcher()
ecs.uidcreator = UIDCreator()

ecs.systems = {}

ecs.nextuid = function()
    return ecs.uidcreator:GetNextUID()
end

ecs.init = function()

end

ecs.update = function(deltaTime,unscaleDeltaTime)

end

ecs.lateupdate = function()

end

ecs.teardown = function()

end

return ecs
