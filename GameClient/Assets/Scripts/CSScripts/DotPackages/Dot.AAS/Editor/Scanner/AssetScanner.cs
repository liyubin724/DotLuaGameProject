using DotEditor.AAS.Filters;
using DotEditor.AAS.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEditor.AAS.Scanner
{
    public class AssetScanner : ScriptableObject
    {
        public ComposedMatcher matcher = new ComposedMatcher();
        public AssetFilter filter = new AssetFilter();

        public void Scan()
        {

        }
    }
}
