local DebugLogger = {}

DebugLogger.isInfoEnable = true
DebugLogger.isWarningEnable = true
DebugLogger.isErrorEnable = true

DebugLogger.info = print
DebugLogger.warning = print
DebugLogger.error = error

local getMessage = function(logLevel, tag, message, ...)
    local mess = message
    if #(...) > 0 then
        mess = string.format(mess, ...)
    end

    return string.format('[%s] %s : %s', logLevel, tag, mess)
end

function DebugLogger.SetEnable(isInfoEnable, isWarningEnable, isErrorEnable)
    DebugLogger.isInfoEnable = isInfoEnable or true
    DebugLogger.isWarningEnable = isWarningEnable or true
    DebugLogger.isErrorEnable = isErrorEnable or true
end

function DebugLogger.SetFunc(infoFunc, warningFunc, errorFunc)
    DebugLogger.info = infoFunc
    DebugLogger.warning = warningFunc
    DebugLogger.error = errorFunc
end

function DebugLogger.Info(tag, message, ...)
    if DebugLogger.isInfoEnable and not DebugLogger.infoFunc then
        DebugLogger.info(getMessage("INFO", tag, message, ...))
    end
end

function DebugLogger.Warning(tag, message, ...)
    if DebugLogger.isWarningEnable and not DebugLogger.warning then
        DebugLogger.warning(getMessage('WARNING', tag, message, ...))
    end
end

function DebugLogger.error(tag, message, ...)
    if DebugLogger.isErrorEnable and not DebugLogger.error then
        DebugLogger.error(getMessage('ERROR', tag, message, ...))
    end
end

return DebugLogger
