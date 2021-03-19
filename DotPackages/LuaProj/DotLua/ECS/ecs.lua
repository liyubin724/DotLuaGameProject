local oop = require('DotLua/OOP/oop')

local CycleTime = oop.using("DotLua/ECS/CycleTime")

local ecs = {}

ecs.time = CycleTime()


ecs.init = function()

end

ecs.update = function(deltaTime,unscaleDeltaTime)
    ecs.time:DoUpdate(deltaTime,unscaleDeltaTime)
end

ecs.lateUpdate = function()

end

ecs.destroy = function()

end




local MessageDispatcher = oop.using('DotLua/Message/MessageDispatcher')
local UIDCreator = oop.using('DotLua/Generic/UIDCreator')
local Context = oop.using('DotLua/ECS/Contexts/Context')


ecs.dispatcher = MessageDispatcher()
ecs.uidCreator = UIDCreator()

ecs.services = {}
ecs.contexts = {}

function ecs.GetDispatcher()
    return ecs.dispatcher
end

function ecs.GetNextUID()
    return ecs.uidCreator:NextUID()
end

function ecs.GetService(name)
    return ecs.services[name]
end

function ecs.AddService(name, instance)
    if ecs.isDebug then
        if not instance then
            ecs.logger.Error('ecs', 'the serviceClass is nil')
            return
        end
        if not instance.GetType or instance.GetType() ~= oop.ObjectType.Instance then
            ecs.logger.Error('ecs', 'the serviceClass is not a class')
            return
        end
    end

    if not ecs.services[name] then
        ecs.services[name] = instance
    else
        ecs.logger.Error('ecs', 'the service has been added')
    end
end

function ecs.RemoveService(name)
    if not ecs.services[name] then
        ecs.services[name] = nil
    end
end

function ecs.HasContext(name)
    return ecs.contexts[name] ~= nil
end

function ecs.GetContext(name)
    return ecs.contexts[name]
end

function ecs.CreateContext(name)
    local context = ecs.contexts[name]
    if not context then
        context = Context(name,ecs.uidCreator)
        ecs.contexts[name] = context
    end
    return context
end

function ecs.DestroyContext(name)
    local context = ecs.GetContext(name)
    if context then
        context:Destroy()
    end
end

return ecs
