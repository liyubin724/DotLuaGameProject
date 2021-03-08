local Component = using('DotLua/ECS/Components/Component')

local RotationComponent =
    class(
    'RotationComponent',
    function(self)
        self.x = 0
        self.y = 0
        self.z = 0
    end,
    Component
)

function RotationComponent:Get()
    return self.x, self.y, self.z
end

function RotationComponent:Set(x, y, z)
    self.x = x
    self.y = y
    self.z = z
end

function RotationComponent:GetX()
    return self.x
end

function RotationComponent:SetX(x)
    self.x = x
end

function RotationComponent:GetY()
    return self.y
end

function RotationComponent:SetY(y)
    self.y = y
end

function RotationComponent:GetZ()
    return self.z
end

function RotationComponent:SetZ(z)
    self.z = z
end


return RotationComponent
