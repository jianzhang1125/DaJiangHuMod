using System;
using System.Globalization;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace TheWorldOfKongfuMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class PluginMain : BaseUnityPlugin
    {
        public static PluginMain Instance { get; private set; }
        public static ConfigEntry<bool> displayTraitChains;
        public static ConfigEntry<bool> displayBookTraitInfo;
        public static ConfigEntry<bool> showMapItemHint;
        public static ConfigEntry<bool> specialEnemy;
        public static ConfigEntry<bool> alwaysSteal;
        public static ConfigEntry<bool> equipEnhance;
        public static ConfigEntry<int> multiProficiency;
        public static ConfigEntry<int> multiFinalAddExp;
        
        private void InitConfigs()
        {
            displayTraitChains = Config.Bind("开关控制", "显示特性组合", true, new ConfigDescription("选择是否显示特性组合信息", new AcceptableValueList<bool>(true, false)));
            displayBookTraitInfo = Config.Bind("开关控制", "显示武功特性", true, new ConfigDescription("是否显示武功特性信息", new AcceptableValueList<bool>(true, false)));
            showMapItemHint = Config.Bind("开关控制", "显示地图隐藏物品", true, new ConfigDescription("是否显示地图隐藏物品", new AcceptableValueList<bool>(true, false)));
            specialEnemy = Config.Bind("开关控制", "快速遭遇特殊敌人", false, new ConfigDescription("是否显快速遭遇特殊敌人", new AcceptableValueList<bool>(true, false)));
            alwaysSteal = Config.Bind("开关控制", "攻击永远偷窃", false, new ConfigDescription("是否显攻击永远偷窃", new AcceptableValueList<bool>(true, false)));
            equipEnhance = Config.Bind("开关控制", "开启强化加强", true, new ConfigDescription("是否开启强化加强", new AcceptableValueList<bool>(true, false)));
            multiProficiency = Config.Bind("经验值板块", "熟练度倍率", 1, new ConfigDescription("熟练度倍率，空挥的经验倍率", new AcceptableValueRange<int>(1, 100)));
            multiFinalAddExp = Config.Bind("经验值板块", "经验值倍率", 1, new ConfigDescription("经验值倍率，人物和学习功法经验值倍率", new AcceptableValueRange<int>(1, 100)));
            
        }


        private void Awake()
        {
            Instance = this;
            InitConfigs();
            try
            {
                Harmony.CreateAndPatchAll(typeof(MultiExp));
                if (equipEnhance.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(EquipEnhance));
                }
                if (displayTraitChains.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(DisplayTraitChainsPatch));
                }
                if (displayBookTraitInfo.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(DisplayBookTraitInfoPatch));
                }
                if (showMapItemHint.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(MapItemHintPatch));
                }
                if (specialEnemy.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(SpecialEnemyPatch));
                }
                if (specialEnemy.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(SpecialEnemyPatch));
                }
                if (alwaysSteal.Value)
                {
                    Harmony.CreateAndPatchAll(typeof(AlwaysStealPatch));
                }

            }
            catch (Exception o)
            {
                LogError("Patch失败了");
                LogError(o);
            }
        }

        public static void LogInfo(object o)
        {
            StringBuilder message = new StringBuilder();
            message.Append("[");
            message.AppendFormat(DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo));
            message.Append("]");
            message.AppendFormat(o.ToString());
            Instance.Logger.LogWarning(message);
        }

        public static void LogError(object o)
        {
            StringBuilder message = new StringBuilder();
            message.Append("*******");
            message.Append("[");
            message.AppendFormat(DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo));
            message.Append("]");
            message.AppendFormat(o.ToString());
            Instance.Logger.LogError(message);
        }
    }
}
