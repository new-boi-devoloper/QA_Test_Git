using MB.QuestLogic;
using UnityEngine;

namespace MB.SO.NpcSos.Main
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/NpcMain/NpcIdSo", order = 0)]
    public class NpcIdSo : ScriptableObject
    {
        public string Name { get; set; }
        public GoodBadEndingType GoodBadNpc { get; set; }
    }
}