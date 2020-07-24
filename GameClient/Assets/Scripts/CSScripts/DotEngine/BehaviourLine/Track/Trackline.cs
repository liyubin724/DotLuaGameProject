using DotEngine.BehaviourLine.Action;
using System.Collections.Generic;

namespace DotEngine.BehaviourLine.Track
{
    public class Trackline
    {
        private List<ActionItem> actionItems = new List<ActionItem>();
        private List<DurationActionItem> runningItems = new List<DurationActionItem>();

        private float elapsedTime = 0.0f;

        private LineContext context = null;
        private TracklineData data = null;
        private ActionItemFactory itemFactory = null;

        public Trackline()
        {
        }

        public void SetData(LineContext context,TracklineData data,float timeScale)
        {
            this.context = context;
            this.data = data;

            itemFactory = context.Get<ActionItemFactory>();

            for(int i =0;i<data.Actions.Count;++i)
            {
                ActionData actionData = data.Actions[i];
                if( actionData == null || actionData.Platform == ActionPlatform.Server)
                {
                    continue;
                }

                ActionItem actionItem = itemFactory.RetainItem(actionData.GetType());
                if(actionItem == null)
                {
                    continue;
                }

                actionItem.SetData(context, actionData, timeScale);
                actionItems.Add(actionItem);
            }
        }

        public void DoUpdate(float deltaTime)
        {
            if (actionItems.Count == 0 && runningItems.Count == 0)
            {
                return;
            }

            elapsedTime += deltaTime;

            while (actionItems.Count > 0)
            {
                ActionItem actionItem = actionItems[0];
                if (actionItem.RealFireTime <= elapsedTime)
                {
                    if (actionItem is DurationActionItem durationActionItem)
                    {
                        durationActionItem.DoEnter();
                        runningItems.Add(durationActionItem);
                    }
                    else if (actionItem is EventActionItem eventActionItem)
                    {
                        eventActionItem.DoTrigger();
                        itemFactory.ReleaseItem(eventActionItem);
                    }
                    actionItems.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }

            if (runningItems.Count > 0)
            {
                for (int i = 0; i < runningItems.Count;)
                {
                    DurationActionItem durationActionItem = runningItems[i];
                    durationActionItem.DoUpdate(deltaTime);

                    if (durationActionItem.RealEndTime <= elapsedTime)
                    {
                        durationActionItem.DoExit();

                        runningItems.RemoveAt(i);
                        itemFactory.ReleaseItem(durationActionItem);
                    }
                    else
                    {
                        ++i;
                    }
                }
            }
        }

        public void DoPause()
        {
            foreach (var item in runningItems)
            {
                item.DoPause();
            }
        }

        public void DoResume()
        {
            foreach (var item in runningItems)
            {
                item.DoResume();
            }
        }

        public void DoDestroy()
        {
            for (int i = runningItems.Count - 1; i >= 0; --i)
            {
                var item = runningItems[0];
                item.DoExit();

                itemFactory.ReleaseItem(item);
            }
            runningItems.Clear();

            for (int i = actionItems.Count - 1; i >= 0; --i)
            {
                var item = actionItems[i];

                itemFactory.ReleaseItem(item);
            }
            actionItems.Clear();

            context = null;
            data = null;
            elapsedTime = 0.0f;
            itemFactory = null;
        }
    }
}
