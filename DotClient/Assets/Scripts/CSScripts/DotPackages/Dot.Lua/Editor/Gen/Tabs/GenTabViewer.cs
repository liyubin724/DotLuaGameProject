using System.Collections.Generic;
using UnityEngine;
using static DotEditor.Lua.Gen.GenSelectionWindow;

namespace DotEditor.Lua.Gen
{
    internal abstract class GenTabViewer
    {
        protected GenConfig genConfig;
        protected List<AssemblyTypeData> assemblyTypes;
        protected string searchText = null;

        protected GenTabViewer(GenConfig config, List<AssemblyTypeData> data)
        {
            genConfig = config;
            assemblyTypes = data;
        }

        protected internal abstract void OnGUI(Rect rect);
        protected internal virtual void OnSearch(string searchText)
        {
            this.searchText = searchText;
        }
    }
}
