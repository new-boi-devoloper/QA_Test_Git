using UnityEngine;

namespace MB.SO.NpcSos.Main
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/NpcMain/NpcPhysicalData", order = 0)]
    public class NpcPhysicalSo : ScriptableObject
    {
        public int baseHealth;
    }
}