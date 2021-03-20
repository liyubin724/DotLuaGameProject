local select = select
local sformat = string.format

local Logger = {}

Logger.isInfoEnable = true
Logger.isWarningEnable = true
Logger.isErrorEnable = true

Logger.info = function(tag, message)
    print(string.format('%s [INFO] %s : %s', os.date("%Y-%m-%d %H:%M:%S",os.time()), tag, message))
end
Logger.warning = function(tag, message)
    print(string.format('%s [WARNING] %s : %s', os.date("%Y-%m-%d %H:%M:%S",os.time()), tag, message))
end
Logger.error = function(tag, message)
    print(string.format('%s [ERROR] %s : %s', os.date("%Y-%m-%d %H:%M:%S",os.time()), tag, message))
end

local getMessage = function(message, ...)
    local mess = message
    if select("#",...) > 0 then
        mess = sformat(mess, ...)
    end

    return mess
end

function Logger.SetEnable(isInfoEnable, isWarningEnable, isErrorEnable)
    Logger.isInfoEnable = isInfoEnable or true
    Logger.isWarningEnable = isWarningEnable or true
    Logger.isErrorEnable = isErrorEnable or true
end

function Logger.SetFunc(infoFunc, warningFunc, errorFunc)
    Logger.info = infoFunc
    Logger.warning = warningFunc
    Logger.error = errorFunc
end

function Logger.Info(tag, message, ...)
    if Logger.isInfoEnable and Logger.info then
        Logger.info(tag, getMessage(message, ...))
    end
end

function Logger.Warning(tag, message, ...)
    if Logger.isWarningEnable and Logger.warning then
        Logger.warning(tag, getMessage(message, ...))
    end
end

function Logger.Error(tag, message, ...)
    if Logger.isErrorEnable and Logger.error then
        Logger.error(tag, getMessage(message, ...))
    end
end

return Logger
