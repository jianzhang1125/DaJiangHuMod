using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace TheWorldOfKongfuMod
{
    /***
            gang_b07Table - 物品描述和物品id 和Relateid
            gang_b03Table - 武功详细和武功id和attached特性id
            gang_b06Table - 特性详细和特性id
     */
    public static class DisplayBookTraitInfoPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PackageController), "UpdatePackageItemDetail")]
        public static void PackageController_UpdatePackageItemDetail_Postfix(PackageController __instance, GameObject btn)
        {
            Text text = __instance.GetFieldValue<GameObject>("DetailPanel")?.transform.Find("Info/Add1")?.GetComponent<Text>();

            if (text != null)
            {
                List<string> appendTraits = getAppendTraits(btn);
                text.text = text.text + "\n<size=20>" + string.Join(",", appendTraits) + "</size>";
                PluginMain.LogInfo(text.text);
            }

            PackageIconController component = btn.GetComponent<PackageIconController>();

            if (component != null && component.itemData.ID.Contains("Traitscroll"))
            {
                gang_b06Table.Row traitRow = CommonResourcesData.b06.Find_id(component.itemData.ID.Split('&')[1]);
                string[] indexs = traitRow.traitEquipIndex.Split('&');
                text.text = text.text + $" [{string.Join(",", indexs)}]";
                PluginMain.LogInfo(text.text);
            }
        }


        private static List<string> getAppendTraits(GameObject btn)
        {
            List<string> list = new List<string>();
            try
            {
                if (btn != null && btn.name != null)
                {
                    PluginMain.LogInfo(btn.name);
                    string[] btnNameArray = btn.name.Split('|');
                    if (btnNameArray.Length != 0 && btnNameArray[0] == "ItemBtn")
                    {
                        //根据item id从b07拿到item row
                        gang_b07Table.Row itemRow = CommonResourcesData.b07.Find_ID(btnNameArray[1]);
                        PluginMain.LogInfo($"Iteam Row 的 " +
                                $"Name{itemRow.Name_Trans} " +
                                $"ID {itemRow.ID} " +
                                $"Relateid{itemRow.Relateid} " +
                                $"Skills1 {itemRow.Skills1} " +
                                $"NSkills1Ecame{itemRow.Skills1Ec} " +
                                $"Style {itemRow.Style} " +
                                $"isAtlas {itemRow.isAtlas}");

                        //根据item 对应的related id从b03拿到wugong row
                        gang_b03Table.Row wugongRow = CommonResourcesData.b03.Find_ID(itemRow.Relateid);
                        if (wugongRow != null)
                        {
                            PluginMain.LogInfo($"Wugong Row的 Name {wugongRow.Name_Trans} ID {wugongRow.ID} AppendTraits {wugongRow.AppendTraits}");
                            string[] appendTraitsArray = wugongRow.AppendTraits.Split('|');
                            if (appendTraitsArray.Length != 0)
                            {
                                int operation = int.Parse(appendTraitsArray[0]);
                                //无特性
                                if (operation == 0 || operation == 999)
                                {
                                    PluginMain.LogInfo($"无特性operation {operation}");
                                    list.Add("无特性");
                                }
                                else
                                {
                                    string[] addedTraits = appendTraitsArray[1].Split('&');
                                    if (!"0".Equals(addedTraits[0]))
                                    {
                                        list.Add("添加");
                                        foreach (string traitID in addedTraits)
                                        {
                                            gang_b06Table.Row traitRow = CommonResourcesData.b06.Find_id(traitID);
                                            if (traitRow != null)
                                            {
                                                string[] indexs = traitRow.traitEquipIndex.Split('&');
                                                PluginMain.LogInfo($"添加Trait Row的 Name {traitRow.name_Trans} ID {traitRow.id} note_Trans {traitRow.note_Trans}");
                                                list.Add($"<color=#008000>{traitRow.name_Trans}-{traitRow.note_Trans} [{string.Join(",", indexs)}]</color>");
                                            }
                                        }
                                    }
                                    string[] deleteTraits = appendTraitsArray[2].Split('&');
                                    if (!"0".Equals(deleteTraits[0]))
                                    {
                                        list.Add("\n删除");
                                        foreach (string traitID in deleteTraits)
                                        {
                                            gang_b06Table.Row traitRow = CommonResourcesData.b06.Find_id(traitID);
                                            if (traitRow != null)
                                            {
                                                string[] indexs = traitRow.traitEquipIndex.Split('&');
                                                PluginMain.LogInfo($"删除Trait Row的 Name {traitRow.name_Trans} ID {traitRow.id} comment_Trans {traitRow.comment_Trans} note_Trans {traitRow.note_Trans}");
                                                list.Add($"<color=#FF0000>{traitRow.name_Trans}-{traitRow.note_Trans}</color>");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                PluginMain.LogError(e.ToString());
            }
            PluginMain.LogInfo(list.ToString());
            return list;
        }
    }
}
