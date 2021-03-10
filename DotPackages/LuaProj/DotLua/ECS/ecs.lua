local oop = require('DotLua/OOP/oop')
local MessageDispatcher = oop.using('DotLua/Message/MessageDispatcher')
local UIDCreator = oop.using('DotLua/Generic/UIDCreator')
local Context = oop.using('DotLua/ECS/Contexts/Context')

local ecs = {}

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
