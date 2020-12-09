namespace DotEngine
{
    public static class Updater
    {
        public static void AddUpdater(IUpdater updater)
        {
            UpdateRunner.GetRunner().AddUpdater(updater);
        }

        public static void RemoveUpdater(IUpdater updater)
        {
            UpdateRunner.GetRunner().RemoveUpdater(updater);
        }
    }
}
