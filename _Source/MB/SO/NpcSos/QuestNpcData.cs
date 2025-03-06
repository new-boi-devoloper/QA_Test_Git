using System;
using System.Collections.Generic;
using MB.QuestLogic;
using MB.QuestLogic.QuestsTargets.Abstract;
using UnityEngine;

namespace MB.SO.NpcSos
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/QuestNpcData/Bob", order = 0)]
    public class QuestNpcData : ScriptableObject
    {
        public List<SideMissionData> missionsList;
        public int completedMissions;
    }

    [Serializable]
    public class SideMissionData
    {
        [field: SerializeField] public int MissionId { get; private set; }
        [TextArea] [field: SerializeField] public string[] MissionIntroDescription { get; private set; }
        [field: SerializeField] public int Reward { get; private set; }
        [field: SerializeField] public SideQuestType Mission { get; private set; }
        [field: SerializeField] public List<QuestTarget> QuestTargets { get; private set; } // Список целей
        [field: SerializeField] public QuestTarget FinalTarget { get; private set; } // Финальная цель

        [field: SerializeField]
        public int RequiredCompletedTargets { get; private set; } // Количество целей для завершения

        [NonSerialized] public bool IsMissionCompleted;

        [NonSerialized] public bool IsMissionInProgress;

        [NonSerialized] public MissionResultType MissionResult;
    }

    public enum SideQuestType
    {
        Collect,
        Kill,
        Check
    }

    // public enum MissionResultType
    // {
    //     Complete, // for meeting the desired by npc result
    //     Failed, // for not meeting the desired by npc result
    //     OppositeComplete, // for meeting the opposite of desired by npc result 
    //     Ignored, // for not taking the mission
    //     Neutral 
    // }
}