using System;
using System.Collections.Generic;
using System.Linq;
using MB.Npc.NpcMain.NpcVariants;
using MB.Player;
using MB.SO.NpcSos;
using MB.SO.NpcSos.Main;
using UnityEngine;
using Zenject;

namespace MB.QuestLogic
{
    public enum GoodBadEndingType
    {
        Good,
        Bad,
        Neutral // Новый тип для проходных заданий
    }

    public enum MissionResultType
    {
        Complete, // for meeting the desired by npc result
        Failed, // for not meeting the desired by npc result
        OppositeComplete, // for meeting the opposite of desired by npc result 
        Ignored, // for not taking the mission
        Neutral
    }

    public class GameProgressTracker
    {
        private readonly NpcSetupSo _npcSetupSo;

        //Dependencies
        private readonly PlayerContainer _playerContainer;

        [Inject]
        public GameProgressTracker(PlayerContainer playerContainer, NpcSetupSo npcSetupSo)
        {
            _playerContainer = playerContainer;
            _npcSetupSo = npcSetupSo;

            NpcMissionResults = new Dictionary<InteractNpcBase, List<GoodBadEndingType>>();
            ActiveQuests = new Dictionary<InteractNpcBase, SideMissionData>();
            FinishedQuest = new Dictionary<InteractNpcBase, SideMissionData>();

            foreach (var sideQuestNpc in _npcSetupSo.setSideQuestNpcs)
                sideQuestNpc.OnMissionFinished += RecordMissionResult;
        }

        //System
        //Results
        public Dictionary<InteractNpcBase, List<GoodBadEndingType>> NpcMissionResults { get; }

        //Active Missions
        public Dictionary<InteractNpcBase, SideMissionData> ActiveQuests { get; }

        //Finished Missions
        public Dictionary<InteractNpcBase, SideMissionData> FinishedQuest { get; }

        public void RegisterAMission(InteractNpcBase npc, SideMissionData newMission)
        {
            ActiveQuests.Add(npc, newMission);
        }

        public void UnRegisterAMission(InteractNpcBase npc, SideMissionData finishedMission)
        {
            ActiveQuests.Remove(npc);
            FinishedQuest.Add(npc, finishedMission);
        }


        // Рассчёт среднего значения для конкретного NPC
        public float CalculateAndSendAverageResult(InteractNpcBase npc)
        {
            if (NpcMissionResults.TryGetValue(npc, out var results))
            {
                var goodCount = results.Count(r => r == GoodBadEndingType.Good);
                var badCount = results.Count(r => r == GoodBadEndingType.Bad);

                var ratio = badCount == 0 ? goodCount : (float)goodCount / badCount;
                ratio = (float)Math.Round(ratio, 2); // Округление до двух знаков

                Debug.Log($"NPC: {npc.name}, Good/Bad Ratio: {ratio}");

                return ratio;
            }

            Debug.Log("wrong npc inserted");
            throw new ArgumentException($"wrong {npc.name} provided (or does not exist)");
        }

        // Рассчёт общего среднего значения для всех NPC
        public float CalculateAndSendAverageResult()
        {
            var totalGoodCount = 0;
            var totalBadCount = 0;

            foreach (var npcResults in NpcMissionResults.Values)
            {
                totalGoodCount += npcResults.Count(r => r == GoodBadEndingType.Good);
                totalBadCount += npcResults.Count(r => r == GoodBadEndingType.Bad);
            }

            var totalRatio = totalBadCount == 0 ? totalGoodCount : (float)totalGoodCount / totalBadCount;
            totalRatio = (float)Math.Round(totalRatio, 2); // Округление до двух знаков

            Debug.Log($"Total Good/Bad Ratio for all NPCs: {totalRatio}");

            return totalRatio;
        }

        // Запись результата миссии
        private void RecordMissionResult(InteractNpcBase npc, GoodBadEndingType result)
        {
            if (!NpcMissionResults.ContainsKey(npc)) NpcMissionResults[npc] = new List<GoodBadEndingType>();
            NpcMissionResults[npc].Add(result);
        }
    }
}