local oop = require('DotLua/OOP/oop')

local function TestObject(obj)
    print('Object.GetClassName = ' .. obj:GetClassName())
    print('Object.GetBaseClass = ' .. tostring(obj:GetBaseClass()))
    print('Object.GetType = ' .. obj:GetType())
    print('Object.IsClass = ' .. tostring(obj:IsClass()))
    print('Object.IsEnum = ' .. tostring(obj:IsEnum()))
    print('Object.IsDelegate = ' .. tostring(obj:IsDelegate()))
    print('Object.IsEvent = ' .. tostring(obj:IsEvent()))
    print('Object.ToString = ' .. tostring(obj:ToString()))
end

local function main()
    local tbl = {}
    tbl.callback = function(tbl, message)
        oop.Logger.Info('TBL', message)
    end

    local tbl2 = {}
    tbl2.callback = function(tbl, message)
        oop.Logger.Info('TBL2', message)
    end

    local event = oop.event()
    event = event + {tbl, tbl.callback}
    event = event + {tbl2, tbl2.callback}

    local GameState = oop.enum('GameState', {'Playing', 'Stopped', 'Paused', 'Finished'})

    event:ActionInvoke(GameState:GetNameByValue(2))
end

main()
