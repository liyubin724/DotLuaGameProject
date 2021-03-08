require('DotLua/oop')

local Object = require('DotLua/Object')
local A =
    class(
    'A',
    function(self)
        self.key = 'TestA'
        self.value = 1
    end
)

function A:PrintMessage()
    print('In A :PrintMessage')
end

local B =
    class(
    'B',
    function(self)
        self.key = 'TestB'
        self.index = 11
    end,
    A
)

function B:PrintMessage()
    print('In B :PrintMessage')
end

local printInfo = function(obj)
    print('className = ' .. obj:GetClassName())
    print('isClass = ' .. tostring(obj:IsClass()))
    print('isInstance = ' .. tostring(obj:IsInstance()))
    print('class = ' .. obj:GetClass():ToString())
    print('isKindOf(Object) = ' .. tostring(obj:IsKindOf(Object)))
    obj:PrintMessage()
end

local testA = function()
    local a1 = A()
    local a2 = A:new()

    print('--------A------------')
    printInfo(A)
    print('---------a1-----------')
    printInfo(a1)
    print('----------a2----------')
    printInfo(a2)
end

local testB = function()
    local b = B()

    print('-----------B----------------')
    printInfo(B)
    print('--------b-------')
    printInfo(b)
end

local cbTarget =
    class(
    'CBTarget',
    function(self)
    end
)

function cbTarget:PrintMessage(message)
    print(message)
end

function cbTarget:PrintEventMessage(message)
    print('PrintEventMessage in CBTarget1.Message = ' .. message)
end

local cbTarget2 =
    class(
    'CBTarget2',
    function(self)
    end,
    cbTarget
)

function cbTarget2:PrintEventMessage(message)
    print('PrintEventMessage in CBTarget2.Message = ' .. message)
end

function cbTarget2:PrintEventMessage2(message)
    print('PrintEventMessage2 in CBTarget2.Message = ' .. message)
end

local testCallback = function()
    local Callback = using('DotLua/Core/Callback')

    local target = cbTarget()
    local cb = Callback(target, target.PrintMessage)
    cb:Invoke('CB1:message not found')

    local target2 = cbTarget2()
    local cb2 = Callback(target2, target2.PrintMessage)
    cb2:Invoke('CB2:message For CB2')
end

local testEvent = function()
    local Event = using('DotLua/Core/Event')
    local event = Event()
    local target = cbTarget()
    local target2 = cbTarget2()
    event = event + {target, target.PrintEventMessage}
    event = event + {target2, target2.PrintEventMessage}
    event = event + {target2, target2.PrintEventMessage2}

    event:Invoke('Event Message')

    event = event - {target2,target2.PrintEventMessage}

    event:Invoke("After Event Message")
end


local PlayerType = enum("PlayerType",{"MainPlayer","Player","NPC"})

local testEnum = function()
    print(tostring(PlayerType))
end

local DebugLogger = using("DotLua/Log/DebugLogger")
local List = using("DotLua/Generic/Collections/List")
local function main()
    local list = List(1,2,3,4,5,6)
    for i, v in ipairs(list) do
        print(string.format("Index = %d,value = %d",i,v))
    end
end

main()
