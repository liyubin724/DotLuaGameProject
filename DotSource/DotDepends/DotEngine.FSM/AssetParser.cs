using DotEngine.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEngine.FSM
{
    public static class AssetParser
    {
        private static Dictionary<int, Type> registerStateDic = new Dictionary<int, Type>();
        private static Dictionary<int, Type> registerConditionDic = new Dictionary<int, Type>();

        public static void AutoRegister()
        {
            Type[] stateTypes = AssemblyUtility.GetCustomAttributeClassTypes<CustomStateAttribute>();
            if (stateTypes != null && stateTypes.Length > 0)
            {
                foreach (var stateType in stateTypes)
                {
                    var attr = stateType.GetCustomAttribute<CustomStateAttribute>();
                    RegisterState(attr.UniqueId, stateType);
                }
            }

            Type[] conditionTypes = AssemblyUtility.GetCustomAttributeClassTypes<CustomConditionAttribute>();
            if (conditionTypes != null && conditionTypes.Length > 0)
            {
                foreach (var conditionType in conditionTypes)
                {
                    var attr = conditionType.GetCustomAttribute<CustomConditionAttribute>();
                    RegisterCondition(attr.UniqueId, conditionType);
                }
            }
        }

        public static void RegisterState(int id, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }
            if (registerStateDic.ContainsKey(id))
            {
                throw new ArgumentException();
            }
            registerStateDic.Add(id, type);
        }

        public static void RegisterCondition(int id, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }
            if (registerStateDic.ContainsKey(id))
            {
                throw new ArgumentException();
            }
            registerConditionDic.Add(id, type);
        }

        public static Machine CreateMachine(AssetData data)
        {
            if (data.MachineDatas == null || data.MachineDatas.Count == 0)
            {
                throw new ArgumentException("");
            }

            string rootMachineGuid = data.HeaderData?.RootMachineGuid;
            if (string.IsNullOrEmpty(rootMachineGuid))
            {
                throw new ArgumentException("");
            }

            Blackboard blackboard = CreateBlackboard(data.BlackboardDatas);
            Dictionary<string, Machine> machineDic = new Dictionary<string, Machine>();
            Dictionary<string, IState> stateDic = new Dictionary<string, IState>();
            Dictionary<string, Transition> transitionDic = new Dictionary<string, Transition>();
            Dictionary<string, ICondition> conditionDic = new Dictionary<string, ICondition>();

            foreach (var cData in data.ConditionDatas)
            {
                ICondition condition = CreateCondition(cData);
                if (condition != null)
                {
                    conditionDic.Add(condition.Guid, condition);
                }
            }

            foreach (var cData in data.ConditionDatas)
            {
                if (cData.fieldNames != null && cData.fieldNames.Count > 0)
                {
                    ICondition targetCondition = conditionDic[cData.Guid];
                    for (int i = 0; i < cData.fieldNames.Count; ++i)
                    {
                        string dependGuid = cData.valueGuids[i];
                        if (!string.IsNullOrEmpty(dependGuid))
                        {
                            ICondition dependCondition = conditionDic[dependGuid];
                            FieldInfo fieldInfo = targetCondition.GetType().GetField(cData.fieldNames[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                            if (fieldInfo != null)
                            {
                                fieldInfo.SetValue(targetCondition, dependCondition);
                            }
                        }
                    }
                }
            }

            foreach (var tData in data.TransitionDatas)
            {
                Transition transition = CreateTransition(tData);
                if (transition != null)
                {
                    if (conditionDic.TryGetValue(tData.ConditionGuid, out var condition))
                    {
                        transition.Condition = condition;
                    }

                    transitionDic.Add(transition.Guid, transition);
                }
            }

            foreach (var sData in data.StateDatas)
            {
                IState state = CreateState(sData);
                if (state != null)
                {
                    stateDic.Add(state.Guid, state);
                }
            }

            Machine rootMachine = null;
            foreach (var mData in data.MachineDatas)
            {
                Machine machine = CreateMachine(mData);
                machine.Blackboard = blackboard;
                if (mData.stateGuids != null && mData.stateGuids.Count > 0)
                {
                    foreach (var stateGuid in mData.stateGuids)
                    {
                        IState state = stateDic[stateGuid];
                        machine.AddState(state);
                    }
                }

                if (mData.transitionGuids != null && mData.transitionGuids.Count > 0)
                {
                    foreach (var transitionGuid in mData.transitionGuids)
                    {
                        Transition transition = transitionDic[transitionGuid];
                        machine.AddTransition(transition);
                    }
                }

                if (mData.Guid == rootMachineGuid)
                {
                    rootMachine = machine;
                }

                machineDic.Add(machine.Guid, machine);
            }

            return rootMachine;
        }

        private static Blackboard CreateBlackboard(List<BlackboardData> datas)
        {
            Blackboard blackboard = new Blackboard();
            if (datas != null && datas.Count > 0)
            {
                foreach (var data in datas)
                {
                    if (data.ValueType == BlackboardValueType.Bool)
                    {
                        blackboard.Add(data.Key, data.BoolValue);
                    }
                    else if (data.ValueType == BlackboardValueType.Float)
                    {
                        blackboard.Add(data.Key, data.FloatValue);
                    }
                    else if (data.ValueType == BlackboardValueType.Int)
                    {
                        blackboard.Add(data.Key, data.IntValue);
                    }
                    else if (data.ValueType == BlackboardValueType.String)
                    {
                        blackboard.Add(data.Key, data.StrValue);
                    }
                }
            }
            return blackboard;
        }

        private static Machine CreateMachine(MachineData data)
        {
            Machine machine = new Machine();
            machine.Guid = data.Guid;
            machine.DisplayName = data.DisplayName;
            machine.InitStateGuid = data.InitStateGuid;
            machine.AutoRunWhenInitlized = data.AutoRunWhenInitlized;
            return machine;
        }

        private static IState CreateState(StateData data)
        {
            if (registerStateDic.TryGetValue(data.ClassId, out var type))
            {
                IState state = (IState)Activator.CreateInstance(type);
                state.DisplayName = data.DisplayName;
                state.Guid = data.Guid;
                return state;
            }
            return null;
        }

        private static Transition CreateTransition(TransitionData data)
        {
            Transition transition = new Transition();
            transition.Guid = data.Guid;
            transition.DisplayName = data.DisplayName;
            transition.FromStateGuid = data.FromStateGuid;
            transition.ToStateGuid = data.ToStateGuid;

            return transition;
        }

        private static ICondition CreateCondition(ConditionData data)
        {
            if (registerConditionDic.TryGetValue(data.ClassId, out var type))
            {
                ICondition condition = (ICondition)Activator.CreateInstance(type);
                condition.Guid = data.Guid;
                condition.DisplayName = data.DisplayName;

                return condition;
            }
            return null;
        }

    }
}
