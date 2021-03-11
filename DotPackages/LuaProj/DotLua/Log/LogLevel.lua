local LogLevel = {
    Info = 1,
    Warning = 2,
    Error = 3
}

function LogLevel.GetNameByValue(value)
    if value == LogLevel.Info then
        return "INFO"
    elseif value == LogLevel.Warning then
        return "WARNING"
    elseif value == LogLevel.Error then
        return "ERROR"
    else
        return "UNKNOWN"
    end
end

return LogLevel
