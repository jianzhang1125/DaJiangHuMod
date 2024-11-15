using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace TheWorldOfKongfuMod
{
    public static class MapItemHintPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapController), "Start")]
        public static void MapController_Start_Postfix(MapController __instance)
        {
            foreach (MapController.Event value in __instance.GetFieldValue<Dictionary<string, MapController.Event>>("events").Values)
            {
                if (!(value.obj == null))
                {
                    string[] array = value.evdata?.action?.Split(new char[1] { '|' });
                    if (array != null && array[0] == "GET" && value.evdata.display == "Prefabs/Field/Dummy")
                    {
                        value.evdata.display = "Prefabs/Effect/GroundLight";
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(gang_e01Table), "Load")]
        public static void gang_e01TableLoadPatch(gang_e01Table __instance, TextAsset csv)
        {
            List<gang_e01Table.Row> fieldValue = __instance.GetFieldValue<List<gang_e01Table.Row>>("rowList");
            for (int i = 0; i < fieldValue.Count; i++)
            {
                string[] array = fieldValue[i].action.Split(new char[1] { '|' });
                if (fieldValue[i].trigger == "CLICK" && array[0] == "GET" && fieldValue[i].display == "Prefabs/Field/Dummy")
                {
                    fieldValue[i].display = "Prefabs/Effect/GroundLight";
                }
            }
        }
    }
}
