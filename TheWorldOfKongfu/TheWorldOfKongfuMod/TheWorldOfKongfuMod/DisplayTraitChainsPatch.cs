using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace TheWorldOfKongfuMod
{

    public static class DisplayTraitChainsPatch
    {
        private static readonly Dictionary<string, List<gang_b06ChainTable.Row>> _chainsCache = new Dictionary<string, List<gang_b06ChainTable.Row>>();

        private static readonly Dictionary<string, List<string>> _hintStringsCache = new Dictionary<string, List<string>>();

        // 创建一个字典来存储数字与中文的映射
        private static readonly Dictionary<int, string> numberMap = new Dictionary<int, string>
            {
                { 1, "一" },
                { 2, "二" },
                { 3, "三" },
                { 4, "四" },
                { 5, "五" },
                { 6, "六" },
                { 7, "七" },
                { 8, "八" },
                { 9, "九" }
            };

        public static List<string> GetMissingTraitHintsForTraitId(string traitId)
        {
            if (_hintStringsCache.ContainsKey(traitId))
            {
                return _hintStringsCache[traitId];
            }
            List<gang_b06ChainTable.Row> chains = FindChainsContainingTrait(traitId);
            List<string> list = ConvertChainsToHintStrings(traitId, chains);
            _hintStringsCache[traitId] = list;
            return list;
        }

        public static List<gang_b06ChainTable.Row> FindChainsContainingTrait(string traitId)
        {
            if (_chainsCache.ContainsKey(traitId))
            {
                return _chainsCache[traitId];
            }
            List<gang_b06ChainTable.Row> list = new List<gang_b06ChainTable.Row>();
            foreach (gang_b06ChainTable.Row row in CommonResourcesData.b06Chain.GetRowList())
            {
                for (int i = 1; i < 10; i++)
                {
                    if (row.nineGridDict[i.ToString()].Split(new char[1] { '|' }).Contains(traitId))
                    {
                        list.Add(row);
                        break;
                    }
                }
            }
            _chainsCache[traitId] = list;
            return list;
        }

        public static List<string> ConvertChainsToHintStrings(string traitId, List<gang_b06ChainTable.Row> chains)
        {
            if (_hintStringsCache.ContainsKey(traitId))
            {
                return _hintStringsCache[traitId];
            }
            List<string> list = new List<string>();
            foreach (gang_b06ChainTable.Row chain in chains)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 1; i < 10; i++)
                {
                    string text = chain.nineGridDict[i.ToString()];
                    if (!text.Equals("0") && !text.Split(new char[1] { '|' }).Contains(traitId))
                    {
                        string conditionNameHint = GetConditionNameHint(text);
                        stringBuilder.Append(GetChineseNumeral(i) + ": " + conditionNameHint + " + ");
                    }
                }
                if (stringBuilder.Length > 0)
                {
                    string item = "【" + chain.name + "】：" + stringBuilder.ToString().TrimEnd(' ', '+');
                    list.Add(item);
                }
            }
            _hintStringsCache[traitId] = list;
            return list;
        }

        private static string GetConditionNameHint(string condition)
        {
            if (condition == "Any")
            {
                return "任意特性";
            }
            if (condition == "NAN")
            {
                return "无特性";
            }
            List<string> values = (from id in condition.Split(new char[1] { '|' })
                                   select CommonResourcesData.b06.Find_id(id).name_Trans).ToList();
            return string.Join(" | ", values);
        }

        private static string GetChineseNumeral(int number)
        {
            // 检查输入的数字是否在字典范围内
            if (numberMap.TryGetValue(number, out string chineseNumber))
            {
                return chineseNumber;
            }
            else
            {
                return number.ToString();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TraitPackageController), "RefreshTraitInfo")]
        public static void TraitPackageController_RefreshTraitInfo_Postfix(TraitPackageController __instance, GameObject traitIcon)
        {
            PluginMain.LogInfo("**** 进入RefreshTraitInfo的Patch");
            Text text = __instance.GetFieldValue<Transform>("TraitInfo")?.Find("TraitInfo/Info")?.GetComponent<Text>();
            string text2 = traitIcon.GetComponentInParent<TraitIconController>()?.traitItemData?.b06Row?.id;
            if (!(text == null) && text2 != null)
            {
                List<string> missingTraitHintsForTraitId = GetMissingTraitHintsForTraitId(text2);
                text.text = text.text + "\n<size=25>" + string.Join("\n", missingTraitHintsForTraitId) + "</size>";
            }
        }
    }
}
