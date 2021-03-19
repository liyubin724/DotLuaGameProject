local oop = require('DotLua/OOP/oop')
local System = oop.using('DotLua/ECS/Systems/System')

local UpdateSystem = oop.using('DotLua/ECS/Systems/LifeCycle/UpdateSystem')
local LateUpdateSystem = oop.using('DotLua/ECS/LifeCycle/LateUpdateSystem')
local FrameSystem = oop.using('DotLua/ECS/Systems/LifeCycle/FrameSystem')

local select = select
local tinsert = table.insert
local tforeach = table.foreach

local MacroSystem =
    oop.class(
    'System',
    function(self, dispatcher, ...)
        self.dispatcher = dispatcher

        self.updateSystems = {}
        self.lateUpdateSystems = {}
        self.frameSystems = {}

        self.systems = {}

        local length = select('#', ...)
        if length > 0 then
            for i = 1, length, 1 do
                local systemClass = select(i, ...)
                self:Add(systemClass)
            end
        end
    end,
    System
)

function MacroSystem:Add(systemClass)
    if oop.isDebug then
        if not systemClass or not oop.isclass(systemClass) or not oop.iskindof(System) then
            oop.error('ECS', 'MacroSystem:Add->the param is not a instance of System')
            return
        end
    end

    local system = systemClass(self.dispatcher)

    if oop.iskindof(system, UpdateSystem) then
        tinsert(self.updateSystems, system)
    elseif oop.iskindof(system, LateUpdateSystem) then
        tinsert(self.lateUpdateSystems, system)
    elseif oop.iskindof(system, FrameSystem) then
        tinsert(self.frameSystems, system)
    else
        oop.error('ECS', 'MacroSystem:Add->the param is not a subclass of System')
        return
    end

    tinsert(self.systems, system)
end

function MacroSystem:DoInitialize()
    tforeach(
        self.systems,
        function(system)
            system:DoInitialize()
        end
    )
end

function MacroSystem:DoUpdate()
    tforeach(
        self.updateSystems,
        function(system)
            system:DoUpdate()
        end
    )
end

function MacroSystem:DoLateUpdate()
    tforeach(
        self.updateSystems,
        function(system)
            system:DoLateUpdate()
        end
    )
end

function MacroSystem:DoFrame()
    tforeach(
        self.frameSystems,
        function(system)
            system:DoFrame()
        end
    )
end

function MacroSystem:DoTeardown()
    tforeach(
        self.systems,
        function(system)
            system:DoTeardown()
        end
    )
end

return MacroSystem
