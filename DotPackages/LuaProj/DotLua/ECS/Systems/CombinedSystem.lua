local oop = require('DotLua/OOP/oop')

local ExecuteSystem = oop.using("DotLua/ECS/Systems/Component/ExecuteSystem")
local InitializeSystem = oop.using("DotLua/ECS/Systems/LifeCycle/InitializeSystem")
local TeardownSystem = oop.using("DotLua/ECS/Systems/LifeCycle/TeardownSystem")
local CleanupSystem = oop.using("DotLua/ECS/Systems/LifeCycle/CleanupSystem")
local MessageSystem = oop.using("DotLua/ECS/Systems/LifeCycle/MessageSystem")

local tinsert = table.insert
local tremove = table.remove

local CombinedSystem =
    oop.class(
    'CombinedSystem',
    function(self,dispatcher)
        self.dispatcher = dispatcher

        self.initializeSystems = {}
        self.executeSystems = {}
        self.cleanupSystems = {}
        self.teardownSystems = {}
        self.messSystems = {}
    end
)

function CombinedSystem:Add(systemClass)
    if systemClass and systemClass.IsKindOf then
        if systemClass:IsKindOf(ExecuteSystem) then
            tinsert(self.executeSystems,systemClass())
        elseif systemClass:IsKindOf(InitializeSystem) then
            tinsert(self.initializeSystems,systemClass())
        elseif systemClass:IsKindOf(TeardownSystem) then
            tinsert(self.teardownSystems,systemClass())
        elseif systemClass:IsKindOf(CleanupSystem) then
            tinsert(self.cleanupSystems,systemClass())
        elseif systemClass:IsKindOf(MessageSystem) then
            tinsert(self.messSystems,systemClass())
        else
            oop.error("CombinedSystem","the \"systemClass\" is not a subclass of System")
        end
    else
        oop.error("CombinedSystem","the \"systemClass\" is not a class")
    end
end

function CombinedSystem:Remove(systemClass)
    if systemClass and systemClass.IsKindOf then
        if systemClass:IsKindOf(ExecuteSystem) then
            self:removeSystem(self.executeSystems,systemClass)
        elseif systemClass:IsKindOf(InitializeSystem) then
            self:removeSystem(self.initializeSystems,systemClass)
        elseif systemClass:IsKindOf(TeardownSystem) then
            self:removeSystem(self.teardownSystems,systemClass)
        elseif systemClass:IsKindOf(CleanupSystem) then
            self:removeSystem(self.cleanupSystems,systemClass)
        elseif systemClass:IsKindOf(MessageSystem) then
            self:removeSystem(self.messSystems,systemClass)
        else
            oop.error("CombinedSystem","the \"systemClass\" is not a subclass of System")
        end
    else
        oop.error("CombinedSystem","the \"systemClass\" is not a class")
    end
end

function CombinedSystem:removeSystem(systems,systemClass)
    for i, s in ipairs(systems) do
        if s:GetClass() == systemClass then
            tremove(systems,i)
            break
        end
    end
end

function CombinedSystem:DoUpdate(deltaTime,unscaleDeltaTime)
    self:DoExecute(deltaTime,unscaleDeltaTime)
end

function CombinedSystem:DoLateUpdate(deltaTime,unscaleDeltaTime)
    self:DoCleanup()
end

function CombinedSystem:DoDestroy()
    self:DoTeardown()
end

function CombinedSystem:DoInitialize()
    for _, system in ipairs(self.teardownSystems) do
        system:DoInitialize()
    end

    for _, system in ipairs(self.messSystems) do
        system:DoInitialize(self.dispatcher)
    end
end

function CombinedSystem:DoExecute()
    for _, system in ipairs(self.executeSystems) do
        system:DoExecute()
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