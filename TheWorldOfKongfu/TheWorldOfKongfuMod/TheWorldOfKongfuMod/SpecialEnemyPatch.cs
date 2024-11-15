using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace TheWorldOfKongfuMod
{
    public static class SpecialEnemyPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(gang_b04RandomFightInfos), "Load")]
        public static void gang_b04RandomFightInfos_Load_Postfix(gang_b04RandomFightInfos __instance, TextAsset csv)
        {
            foreach (gang_b04RandomFightInfos.Row item in __instance.GetFieldValue<List<gang_b04RandomFightInfos.Row>>("rowList"))
            {
                if (!(item.Unique == "0"))
                {
                    item.RequireStep = "0";
                    item.BattleRate = (item.BattleRate.ToFloat() * 10f).ToString();
                }
            }
        }
    }
}
