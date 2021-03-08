local Component = using('DotLua/ECS/Components/Component')

local PositionComponent =
    class(
    'PositionComponent',
    function(self)
        self.x = 0
        self.y = 0
        self.z = 0
    end,
    Component
)

function PositionComponent:Get()
    return self.x, self.y, self.z
end

function PositionComponent:Set(x, y, z)
    self.x = x
    self.y = y
    self.z = z
end

function PositionComponent:GetX()
    return self.x
end

function PositionComponent:SetX(x)
    self.x = x
end

function PositionComponent:GetY()
    return self.y
end

function PositionComponent:SetY(y)
    self.y = y
end

function PositionComponent:GetZ()
    return self.z
end

function PositionComponent:SetZ(z)
    self.z = z
end

return PositionComponent
