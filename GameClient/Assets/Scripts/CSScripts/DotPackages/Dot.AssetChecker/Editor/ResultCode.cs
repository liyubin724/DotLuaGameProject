using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.AssetChecker
{
    public static class ResultCode
    {
        public static readonly int SUCCESS = 0;

        public static readonly int ERR_ANALYSE_ASSET_PATH_EMPTY = 1000;
        public static readonly int ERR_ANALYSE_ASSET_NOT_FOUND = 1001;
        public static readonly int ERR_ANALYSE_ASSET_NOT_UNITYOBJECT = 1002;

        public static readonly int ERR_CHECK_TEXTURE_INVALID = 2001;
        public static readonly int ERR_CHECK_TEXTURE_LT_MAX_WIDTH = 2002;
        public static readonly int ERR_CHECK_TEXTURE_LT_MAX_HEIGHT = 2003;


        public static readonly int ERR_OPER_ASSET_IMPORT_INVALID = 20001;
    }
}
