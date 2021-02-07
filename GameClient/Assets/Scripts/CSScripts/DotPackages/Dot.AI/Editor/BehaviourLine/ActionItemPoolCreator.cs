using DotEngine.BehaviourLine.Action;
using DotEngine.Context;
using DotEngine.Utilities;
using DotTool.ScriptGenerate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class ActionItemPoolCreator
    {
        [MenuItem("Game/Behaviour Line/Create Item Pool")]
        public static void Create()
        {
            string spaceName = "Game.Timeline";
            string outputDir = "D:/";
            string templateFilePath = "D:/WorkSpace/DotGameProject/DotGameScripts/Dot/DotEditor/action-item-pool-template.txt";

            CreateItemPool(spaceName, outputDir, templateFilePath);
        }

        public static void CreateItemPool(string spaceName,string outputDir,string templateFilePath)
        {
            Type[] itemTypes = AssemblyUtility.GetDerivedTypes(typeof(ActionItem));
            if (itemTypes == null || itemTypes.Length == 0)
            {
                Debug.LogError($"ActionItemPoolCreator::CreateItemPool->ActionItem is not found!");
                return;
            }

            Dictionary<Type, Type> dataToItemTypeDic = new Dictionary<Type, Type>();
            foreach (var itemType in itemTypes)
            {
                ActionItemBindDataAttribute attr = itemType.GetCustomAttribute<ActionItemBindDataAttribute>();
                if (attr == null)
                {
                    Debug.LogError($"ActionItemPoolCreator::CreateItemPool->Attribute is not found.itemType = {itemType.FullName}");
                    continue;
                }

                dataToItemTypeDic.Add(attr.DataType, itemType);
            }

            StringContextContainer context = new StringContextContainer();
            context.Add("spaceName", spaceName);
            context.Add("dataToItemTypeDic", dataToItemTypeDic);

            string templateContent = File.ReadAllText(templateFilePath);

            string outputFilePath = $"{outputDir}/ActionItemPoolRegister.cs";
            string outputContent = TemplateEngine.Execute(context, templateContent, new string[0]);
            File.WriteAllText(outputFilePath, outputContent);
        }
    }
}
