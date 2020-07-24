using DotEngine.Log;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace DotEngine.Lua.UI.ListView
{
    [RequireComponent(typeof(ScrollRect))]
    public class LuaUIListView : LoopListView2
    {
        private const string CONTROLLER_NAME = "view";
        private const string GET_ITEM_FUNC_NAME = "GetItemName";
        private const string SET_DATA_FUNC_NAME = "SetItemData";

        public LuaBindScript bindScript = new LuaBindScript();

        protected virtual void Awake()
        {
            if(bindScript.InitLua())
            {
                bindScript.CallAction(LuaConst.AWAKE_FUNCTION_NAME);
                OnInitFinished();
            }
        }

        protected virtual void OnInitFinished()
        {
            bindScript.ObjTable.Set(CONTROLLER_NAME, this);
        }

        protected virtual void Start()
        {
            bindScript.CallAction(LuaConst.START_FUNCTION_NAME);
        }

        protected virtual void OnDestroy()
        {
            if(bindScript.IsValid())
            {
                bindScript.CallAction(LuaConst.DESTROY_FUNCTION_NAME);
                bindScript.Dispose();
            }
        }

        public void InitListView(int tototalCount)
        {
            InitListView(tototalCount, OnGetItemByIndex);
        }

        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
        {
            string itemName = bindScript.CallFunc<int,string>(GET_ITEM_FUNC_NAME, index);
            if (string.IsNullOrEmpty(itemName))
            {
                return null;
            }

            LoopListViewItem2 item = listView.NewListViewItem(itemName);
            if(item is LuaUIListViewItem viewItem)
            {
                bindScript.CallAction<int, LuaTable>(SET_DATA_FUNC_NAME, index, viewItem.ObjTable);
            }
            else
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaListView::OnGetItemByIndex->the behaviour of ObjectBind is Null");
            }
            return item;
        }

        //public LuaTable GetShownItemByItemIndex(int itemIndex)
        //{
        //    LoopListViewItem2 item = listView?.GetShownItemByItemIndex(itemIndex);
        //    if (item != null && item.LuaListViewItem != null)
        //    {
        //        return item.LuaListViewItem.ObjTable;
        //    }
        //    return null;
        //}

        //public LuaTable GetShownItemByIndex(int index)
        //{
        //    LoopListViewItem2 item = listView?.GetShownItemByIndex(index);
        //    if (item != null && item.LuaListViewItem != null)
        //    {
        //        return item.LuaListViewItem.ObjTable;
        //    }
        //    return null;
        //}

        //public void RefreshItemByItemIndex(int itemIndex)
        //{
        //    listView?.RefreshItemByItemIndex(itemIndex);
        //}

        //public void RefreshAllShownItem()
        //{
        //    listView?.RefreshAllShownItem();
        //}

        //public void MovePanelToItemIndex(int itemIndex, float offset)
        //{
        //    listView?.MovePanelToItemIndex(itemIndex, offset);
        //}

        //public void OnItemSizeChanged(int itemIndex)
        //{
        //    listView?.OnItemSizeChanged(itemIndex);
        //}
    }
}
