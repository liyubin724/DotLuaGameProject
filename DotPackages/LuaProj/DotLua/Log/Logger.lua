local LogLevel = require('DotLua/Log/LogLevel')

local Logger = {}

Logger.isInfoEnable = true
Logger.isWarningEnable = true
Logger.isErrorEnable = true

local print = function(logLevel, tag, message)
    print(string.format('[%s] %s : %s', LogLevel.GetNameByValue(logLevel), tag, message))
end

Logger.info = print
Logger.warning = print
Logger.error = print

local getMessage = function(message, ...)
    local mess = message
    if #(...) > 0 then
        mess = string.format(mess, ...)
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
    if Logger.isInfoEnable and not Logger.infoFunc then
        Logger.info(LogLevel.Info, tag, getMessage(message, ...))
    end
end

function Logger.Warning(tag, message, ...)
    if Logger.isWarningEnable and not Logger.warning then
        Logger.warning(LogLevel.Warning, tag, getMessage(message, ...))
    end
end

function Logger.Error(tag, message, ...)
    if Logger.isErrorEnable and not Logger.error then
        Logger.error(LogLevel.Error, tag, getMessage(message, ...))
    end
end

if not _G.Logger then
    _G.Logger = Logger
end

return Logger
