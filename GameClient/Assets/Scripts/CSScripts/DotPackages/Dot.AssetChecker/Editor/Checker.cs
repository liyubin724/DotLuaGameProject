namespace DotEditor.AssetChecker
{
    public class Checker
    {
        public string name = "Checker";
        public bool enable = true;

        public Matcher matcher = new Matcher();
        public Analyser analyser = new Analyser();
        public Operater operater = new Operater();

        public bool DoMatch(string assetPath)
        {
            if(!enable)
            {
                return false;
            }
            return matcher.IsMatch(assetPath);
        }

        public bool DoAnalyse(string assetPath,ref int errorCode)
        {
            return analyser.Analyse(assetPath, ref errorCode);
        }

        public void DoOperate(string assetPath)
        {
            operater.Operate(assetPath);
        }
    }
}
