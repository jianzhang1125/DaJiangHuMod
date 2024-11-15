using HarmonyLib;
using System.Collections.Generic;


namespace TheWorldOfKongfuMod
{
    public static class AlwaysStealPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SkillTraitEquipManager), "RunSkill")]
        public static void SkillTraitEquipManager_RunSkill_Prefix(BattleObject _attacker, BattleObject _defender)
        {
            if (_attacker.race != "player" || _defender.race == "player")
            {
                return;
            }
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < _defender.m_StealableItemsName.Count; i++)
            {
                if (_defender.m_StealableItemsNum[i] != 0)
                {
                    string item = _defender.m_StealableItemsName[i].Item;
                    int num = _defender.m_StealableItemsNum[i];
                    SharedData.Instance().PackageAdd(item, num);
                    if (dictionary.ContainsKey(item))
                    {
                        dictionary[item] += num;
                    }
                    else
                    {
                        dictionary.Add(item, num);
                    }
                    _defender.m_StealableItemsNum[i] = 0;
                }
            }
            string text = _attacker.charadata.Indexs_Name["Name"].stringValue + " " + CommonFunc.I18nGetLocalizedValue("I18N_StalSuccess");
            List<string> list = new List<string>();
            string icon = string.Empty;
            foreach (KeyValuePair<string, int> item2 in dictionary)
            {
                gang_b07Table.Row row = CommonResourcesData.b07.Find_ID(item2.Key);
                if (row != null)
                {
                    list.Add($"<color=#f0e352>{row.Name_Trans}</color> * {item2.Value}");
                    icon = row.BookIcon;
                }
            }
            if (list.Count > 0)
            {
                string info = text + " " + string.Join(", ", list) + "！";
                _attacker.m_MenuController.OpenInteruptInfo(_attacker, info, icon);
                _attacker.InteruptIn(BattleObjectState.ActionOver);
            }
        }
    }
}
