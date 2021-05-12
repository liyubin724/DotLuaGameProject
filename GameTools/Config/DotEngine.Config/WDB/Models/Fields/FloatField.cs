﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
{

    [WDBFieldLink(WDBFieldType.Float)]
    public class FloatField : WDBField
    {
        protected override string GetInnerDefaultValue()
        {
            return "0.0";
        }
    }
}
