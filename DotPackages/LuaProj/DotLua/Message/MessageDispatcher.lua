local ObjectPool = using('DotLua/Pool/ObjectPool')
local MessageListener = using('DotLua/Message/MessageListener')

local tinsert = table.insert
local tremove = table.remove
local tkeys = table.keys

local MessageDispatcher =
    class(
    'MessageDispatcher',
    function(self)
        self.listenerPool = ObjectPool(MessageListener)
        self.listenerMapList = {}
    end
)

function MessageDispatcher:RegistListener(name, receiver, func, userdata)
    local listenerList = self.listenerMapList[name]
    if not listenerList then
        listenerList = {}
        self.listenerMapList[name] = listenerList
    end

    local listener = self.listenerPool:Get()
    listener:Set(name, receiver, func, userdata)
    tinsert(listenerList, listener)
end

function MessageDispatcher:UnregistListener(name, receiver, func)
    local listenerList = self.listenerMapList[name]
    if listenerList then
        for i = #listenerList, 1, -1 do
            local listener = listenerList[i]
            if listener.GetReceiver() == receiver and listener.GetFunc() == func then
                tremove(listenerList, i)

                self.listenerPool:Release(listener)
            end
        end
    end
end

function MessageDispatcher:UnregistAll(name)
    local listenerList = self.listenerMapList[name]
    if listenerList then
        for i = #(listenerList), 1, -1 do
            self.listenerPool:Release(listenerList[i])
        end
        self.listenerMapList[name] = nil
    end
end

function MessageDispatcher:Trigger(name, ...)
    local listenerList = self.listenerMapList[name]
    if listenerList then
        for _, listener in ipairs(listenerList) do
            listener:Invoke(...)
        end
    end
end

function MessageDispatcher:Clear()
    local names = tkeys(self.listenerMapList)
    for _, name in ipairs(names) do
        local listenerList = self.listenerMapList[name]
        for i = #(listenerList), 1, -1 do
            self.listenerPool:Release(listenerList[i])
        end
        self.listenerMapList[name] = nil
    end
end

return MessageDispatcher
