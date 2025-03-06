using System;
using System.Collections.Generic;
using UnityEngine;

namespace MB.SO.EnemySos.Main
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/EnemyMain/EnemySetupSo", order = 0)]
    public class EnemySetupSo : ScriptableObject
    {
        public List<EnemySetup> enemySetupData;

        [Serializable]
        public class EnemySetup
        {
            // [field: SerializeField] public Npc.NpcMain.Npc Npc { get; private set; }
        }
    }
}