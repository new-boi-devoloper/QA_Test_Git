using System.Collections.Generic;
using MB.Npc.Npcs.InteractableNpcs;
using UnityEngine;

namespace MB.SO.NpcSos.Main
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/NpcMain/NpcSetupSo", order = 0)]
    public class NpcSetupSo : ScriptableObject
    {
        public List<SideQuestInteractNpc> setSideQuestNpcs;
        public List<LoreQuestNpc> setLoreQuestNpcs;
    }
}