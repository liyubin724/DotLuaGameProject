local oop = require('DotLua/OOP/oop')
local MessageDispatcher = oop.using('DotLua/Message/MessageDispatcher')
local DebugLogger = oop.using('DotLua/Log/DebugLogger')
local UIDCreator = oop.using("DotLua/ECS/Services/UIDCreatorService")

local ecs = {}
ecs.isDebug = oop.isDebug
ecs.logger = DebugLogger
ecs.dispatcher = MessageDispatcher()
ecs.uidCreator = UIDCreator()

ecs.services = {}

function ecs.GetIsDebug()
    return ecs.isDebug
end

function ecs.GetLogger()
    return ecs.logger
end

function ecs.GetDispatcher()
    return ecs.dispatcher
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


ecs.AddService(UIDCreatorService.NAME, UIDCreatorService())
return ecs
