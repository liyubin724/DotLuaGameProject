local oop = require('DotLua/OOP/oop')

local ExecuteSystem = oop.using('DotLua/ECS/Systems/Component/ExecuteSystem')
local InitializeSystem = oop.using('DotLua/ECS/Systems/LifeCycle/InitializeSystem')
local TeardownSystem = oop.using('DotLua/ECS/Systems/LifeCycle/TeardownSystem')
local CleanupSystem = oop.using('DotLua/ECS/Systems/LifeCycle/CleanupSystem')
local MessageSystem = oop.using('DotLua/ECS/Systems/LifeCycle/MessageSystem')

local tinsert = table.insert
local tremovevalue = table.removevalue

local CombinedSystem =
    oop.class(
    'CombinedSystem',
    function(self, dispatcher)
        self.dispatcher = dispatcher

        self.initializeSystems = {}
        self.executeSystems = {}
        self.cleanupSystems = {}
        self.teardownSystems = {}
        self.messSystems = {}
    end
)

function CombinedSystem:LaunchSubsystem(system)
    if oop.isinstance(system) then
        if oop.iskindof(system, ExecuteSystem) then
            tinsert(self.executeSystems, system)
        elseif oop.iskindof(system, InitializeSystem) then
            tinsert(self.initializeSystems, system)
        elseif oop.iskindof(system, TeardownSystem) then
            tinsert(self.teardownSystems, system)
        elseif oop.iskindof(system, CleanupSystem) then
            tinsert(self.cleanupSystems, system)
        elseif oop.iskindof(system, MessageSystem) then
            system:SetDispatcher(self.dispatcher)
            tinsert(self.messSystems, system)
        else
            oop.error('CombinedSystem', 'the "system" is not a subclass of System')
        end
    else
        oop.error('CombinedSystem', 'the "system" is not a instance')
    end
end

function CombinedSystem:AbortSubsystem(system)
    if oop.isinstance(system) then
        if oop.iskindof(system, ExecuteSystem) then
            tremovevalue(self.executeSystems, system)
        elseif oop.iskindof(system, InitializeSystem) then
            tremovevalue(self.initializeSystems, system)
        elseif oop.iskindof(system, TeardownSystem) then
            tremovevalue(self.teardownSystems, system)
        elseif oop.iskindof(system, CleanupSystem) then
            tremovevalue(self.cleanupSystems, system)
        elseif oop.iskindof(system, MessageSystem) then
            tremovevalue(self.messSystems, system)
        else
            oop.error('CombinedSystem', 'the "system" is not a subclass of System')
        end
    else
        oop.error('CombinedSystem', 'the "system" is not a instance')
    end
end

function CombinedSystem:DoInitialize()
    for _, system in ipairs(self.teardownSystems) do
        system:DoInitialize()
    end

    for _, system in ipairs(self.messSystems) do
        system:DoInitialize(self.dispatcher)
    end
end

function CombinedSystem:DoExecute(deltaTime, unscaleDeltaTime)
    for _, system in ipairs(self.executeSystems) do
        system:DoExecute(deltaTime, unscaleDeltaTime)
    end
end

function CombinedSystem:DoCleanup()
    for _, system in ipairs(self.cleanupSystems) do
        system:DoCleanup()
    end
end

function CombinedSystem:DoTeardown()
    for _, system in ipairs(self.teardownSystems) do
        system:DoTeardown()
    end

    for _, system in ipairs(self.messSystems) do
        system:DoTeardown()
    end
end

return CombinedSystem
