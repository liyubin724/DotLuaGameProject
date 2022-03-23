using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Core
{
    public class DependenceData<T>
    {
        public T Main;
        public T Depend;
    }

    public class DependenceGroupData<T>
    {
        public T Main;
        public T[] Depends;
    }

    public static class CircularDependencyChecker
    {
        public static bool IsInCircular<T>(IList<DependenceData<T>> datas)
        {
            if (datas == null || datas.Count == 0)
            {
                return false;
            }

            CreateDependenceData(datas, out var dependenceDic, out var indegreeDic);

            Stack<T> zeroIndegreeStack = new Stack<T>();
            if (!PickZeroIndegreeInToStack(indegreeDic, zeroIndegreeStack))
            {
                return false;
            }

            return IsInCircular(dependenceDic, indegreeDic, zeroIndegreeStack);
        }

        public static bool IsInCircular<T>(IList<DependenceGroupData<T>> datas)
        {
            if (datas == null || datas.Count == 0)
            {
                return false;
            }

            CreateDependenceData(datas, out var dependenceDic, out var indegreeDic);

            Stack<T> zeroIndegreeStack = new Stack<T>();
            if (!PickZeroIndegreeInToStack(indegreeDic, zeroIndegreeStack))
            {
                return false;
            }

            return IsInCircular(dependenceDic, indegreeDic, zeroIndegreeStack);
        }

        private static void CreateDependenceData<T>(IList<DependenceData<T>> datas, out Dictionary<T, List<T>> dependenceDic, out Dictionary<T, int> indegreeDic)
        {
            dependenceDic = new Dictionary<T, List<T>>();
            indegreeDic = new Dictionary<T, int>();

            foreach (var data in datas)
            {
                if (dependenceDic.TryGetValue(data.Main, out var depends))
                {
                    depends = new List<T>();
                    dependenceDic.Add(data.Main, depends);
                }
                if (!depends.Contains(data.Depend))
                {
                    depends.Add(data.Depend);

                    if (indegreeDic.ContainsKey(data.Depend))
                    {
                        indegreeDic[data.Depend] += 1;
                    }
                    else
                    {
                        indegreeDic.Add(data.Depend, 1);
                    }
                }

                if (!indegreeDic.ContainsKey(data.Main))
                {
                    indegreeDic.Add(data.Main, 0);
                }
            }
        }

        private static void CreateDependenceData<T>(IList<DependenceGroupData<T>> datas, out Dictionary<T, List<T>> dependenceDic, out Dictionary<T, int> indegreeDic)
        {
            dependenceDic = new Dictionary<T, List<T>>();
            indegreeDic = new Dictionary<T, int>();

            foreach (var data in datas)
            {
                if (dependenceDic.TryGetValue(data.Main, out var depends))
                {
                    depends = new List<T>();
                    dependenceDic.Add(data.Main, depends);
                }
                foreach(var depend in data.Depends)
                {
                    if (!depends.Contains(depend))
                    {
                        depends.Add(depend);

                        if (indegreeDic.ContainsKey(depend))
                        {
                            indegreeDic[depend] += 1;
                        }
                        else
                        {
                            indegreeDic.Add(depend, 1);
                        }
                    }
                }
                
                if (!indegreeDic.ContainsKey(data.Main))
                {
                    indegreeDic.Add(data.Main, 0);
                }
            }
        }

        private static bool PickZeroIndegreeInToStack<T>(Dictionary<T, int> indegreeDic, Stack<T> zeroIndegreeStack)
        {
            var elements = (from kvp in indegreeDic where kvp.Value == 0 select kvp.Key).ToArray();
            if (elements == null || elements.Length == 0)
            {
                return false;
            }
            foreach (var element in elements)
            {
                indegreeDic.Remove(element);
                zeroIndegreeStack.Push(element);
            }
            return true;
        }

        private static bool IsInCircular<T>(Dictionary<T, List<T>> dependenceDic, Dictionary<T, int> indegreeDic, Stack<T> zeroIndegreeStack)
        {
            while (zeroIndegreeStack.Count > 0)
            {
                T element = zeroIndegreeStack.Pop();
                List<T> depends = dependenceDic[element];
                foreach (var depend in depends)
                {
                    indegreeDic[depend] -= 1;
                }
                PickZeroIndegreeInToStack(indegreeDic, zeroIndegreeStack);
            }

            if (indegreeDic.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
