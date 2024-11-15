using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Random;

namespace TheWorldOfKongfuMod
{
    public static class MultiExp
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BattleObject), "SkillLevelUpCheckProcess")]
        public static bool BattleObject_SkillLevelUpCheckProcess_Prefix(BattleObject __instance)
        {
            if (__instance == null || __instance.m_State != BattleObjectState.ActionOverPost || __instance.race != "player"
                || __instance.m_BattleController == null || !__instance.m_BattleController.isRoundActionFinish())
            {
                return true;
            }

            try
            {
                if (__instance.m_FinalAddExp > 0)
                {
                    PluginMain.LogInfo($"经验值原先为 {__instance.m_FinalAddExp}");
                    __instance.m_FinalAddExp = __instance.m_FinalAddExp * PluginMain.multiFinalAddExp.Value;
                    PluginMain.LogInfo($"经验值增加到 {__instance.m_FinalAddExp}");
                    PluginMain.LogInfo($"{__instance.charadata.m_Id} 经验值倍率修改完成");
                }
            }
            catch (Exception e)
            {
                PluginMain.LogError(e.ToString());
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BattleObject), "SkillLevelUpCheckProcess")]
        public static void BattleObject_SkillLevelUpCheckProcess_Postfix(BattleObject __instance)
        {
            if (__instance == null || __instance.m_State != BattleObjectState.ActionOverPost || __instance.race != "player"
                || __instance.m_BattleController == null || !__instance.m_BattleController.isRoundActionFinish())
            {
                return;
            }

            try
            {
                if (__instance.m_SkillRow != null && __instance.m_SkillRow.proficiency >= 0 && __instance.m_SkillRow.proficiency < 100)
                {
                    PluginMain.LogInfo($"熟练度原先为 {__instance.m_SkillRow.proficiency}");
                    __instance.m_SkillRow.proficiency = __instance.m_SkillRow.proficiency + PluginMain.multiProficiency.Value;
                    PluginMain.LogInfo($"熟练度增加到 {__instance.m_SkillRow.proficiency}");
                    PluginMain.LogInfo($"{__instance.charadata.m_Id} {__instance.m_SkillRow.kf.Name_Trans}熟练度倍率完成");
                }
            }
            catch (Exception e)
            {
                PluginMain.LogError(e.ToString());
            }
        }
    }
}
