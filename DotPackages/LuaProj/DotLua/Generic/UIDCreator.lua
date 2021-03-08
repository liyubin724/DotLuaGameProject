local UIDCreator =
    class(
    'UIDCreator',
    function(self,start)
        self.uid = start or 0
    end
)

function UIDCreator:NextUID()
    self.uid = self.uid + 1

    return self.uid
end

return UIDCreator
