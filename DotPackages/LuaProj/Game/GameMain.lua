local ecs = require('DotLua/ECS/ecs')

GameMain = {}
GameMain.__index = GameMain

function GameMain.Init()
    require('Game/Init')

    ecs.init()
end

function GameMain.Update(deltaTime, unscaleDeltaTime)
    ecs.update(deltaTime, unscaleDeltaTime)
end

function GameMain.LateUpdate()
    ecs.lateupdate()
end

function GameMain.Destroy()
    ecs.teardown()
end
