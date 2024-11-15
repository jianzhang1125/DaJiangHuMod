using HarmonyLib;
using System.Globalization;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace TheWorldOfKongfuMod
{
    public static class EquipEnhance
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EvolutionEqNewController), "MaterialAdd")]
        public static void EvolutionEqNewController_MaterialAdd_Postfix(EvolutionEqNewController __instance, ref gang_b13Table.Row _b13Row, ref gang_b02Table.Row _b02row)
        {
            PluginMain.LogInfo(_b02row.ID);
            PluginMain.LogInfo(_b02row.Name_Trans);
            Type type = _b02row.GetType();
            FieldInfo fieldInfo = null;
            FieldInfo fieldInfo2 = null;
            for (int i = 1; i <= 5; i++)
            {
                fieldInfo = type.GetField("add" + i);
                fieldInfo2 = type.GetField("Attribute" + i);
                PluginMain.LogInfo($"{fieldInfo.GetValue(_b02row).ToString()} {fieldInfo2.GetValue(_b02row).ToString()}");

                List<string> list1 = new List<string> { "MOR", "EXPup"};
                if (list1.Contains((fieldInfo.GetValue(_b02row).ToString())))
                {
                    float currentValue = float.Parse(fieldInfo2.GetValue(_b02row).ToString(), CultureInfo.InvariantCulture);
                    fieldInfo2.SetValue(_b02row, (1 + currentValue).ToString());
                }

                //List<string> list2 = new List<string> { "Speed", "Combo" };
                List<string> list2 = new List<string> { "Speed" };
                if (list2.Contains((fieldInfo.GetValue(_b02row).ToString())))
                {
                    float currentValue = float.Parse(fieldInfo2.GetValue(_b02row).ToString(), CultureInfo.InvariantCulture);
                    fieldInfo2.SetValue(_b02row, (0.01 + currentValue).ToString());
                }

                List<string> list3 = new List<string> { "Steal", "Melody", "WIL", "LER" };
                if (list3.Contains((fieldInfo.GetValue(_b02row).ToString())))
                {
                    float currentValue = float.Parse(fieldInfo2.GetValue(_b02row).ToString(), CultureInfo.InvariantCulture);
                    fieldInfo2.SetValue(_b02row, (1 + currentValue).ToString());
                }
            }
        }
    }
}
