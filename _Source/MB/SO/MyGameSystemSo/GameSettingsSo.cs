using UnityEngine;

namespace MB.SO.MyGameSystemSo
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/System/GameSettingsSo", order = 0)]
    public class GameSettingsSo : ScriptableObject
    {
        public int npcTurnOnDistance = 30;
        public int enemyTurnOnDistance;
    }
}