using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using MB.Npc.NpcMain.NpcVariants;
using MB.QuestLogic;
using MB.SO.NpcSos;
using MB.SO.NpcSos.Main;
using MB.SO.QuestSo;
using MB.UI.DialogUI;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace MB.Npc.Npcs.InteractableNpcs
{
    public class LoreQuestNpc : InteractNpcBase
    {
        [field: SerializeField] private QuestNpcData questData;
        [field: SerializeField] private NpcIdSo npcId;
        [field: SerializeField] private DialogData dialogData;
        private DialogController _dialogController;

        //dependencies
        private GameProgressTracker _gameProgressTracker;

        public override CapsuleCollider NpcCollider { get; protected set; }
        public override NavMeshAgent Agent { get; protected set; }

        private void Start()
        {
            NpcCollider = GetComponent<CapsuleCollider>();
            Agent = GetComponent<NavMeshAgent>();

            questData.missionsList.ForEach(mission => mission.IsMissionCompleted = false);
            AbleToTalk = questData.missionsList.Any(mission => !mission.IsMissionCompleted);
        }

        public event Action<InteractNpcBase, GoodBadEndingType> OnMissionFinished;

        [Inject]
        public void Construct(GameProgressTracker gameProgressTracker, DialogController dialogController)
        {
            _gameProgressTracker = gameProgressTracker;
            _dialogController = dialogController;
        }

        protected override async UniTask OnHasDeal()
        {
            var nextMission = questData.missionsList
                .OrderBy(mission => mission.MissionId)
                .FirstOrDefault(mission => !mission.IsMissionCompleted && !mission.IsMissionInProgress);

            if (nextMission == null)
            {
                Debug.Log("No available missions.");
                return;
            }

            // Запуск диалога
            var isMissionAccepted = await _dialogController.InvokeDialog(dialogData);
            if (!isMissionAccepted)
            {
                Debug.Log("Mission declined by player.");
                return;
            }

            Debug.Log($"нпс получил {isMissionAccepted}");

            // Регистрация миссии
            nextMission.IsMissionInProgress = true;
            _gameProgressTracker.RegisterAMission(this, nextMission);

            // Сброс IsInteracting, чтобы разблокировать управление
            IsInteracting = false;
            Debug.Log($"но мы кажется не вышли из OnHasDeal {IsInteracting}");

            // Ожидание завершения миссии
            await AwaitForMissionCompletion(nextMission);

            // Завершение миссии
            nextMission.IsMissionCompleted = true;
            _gameProgressTracker.UnRegisterAMission(this, nextMission);
        }

        private async UniTask AwaitForMissionCompletion(SideMissionData mission)
        {
            // Логика ожидания завершения миссии
            var completedTargets = 0;
            foreach (var target in mission.QuestTargets)
                if (await target.WaitForTargetToBeReached())
                {
                    var missionResult = DetermineMissionResult(npcId.GoodBadNpc, target.TargetType);
                    if (missionResult != MissionResultType.Neutral && missionResult == MissionResultType.Complete)
                        completedTargets++;
                }

            if (completedTargets >= mission.RequiredCompletedTargets)
                mission.MissionResult = MissionResultType.Complete;
            else
                mission.MissionResult = MissionResultType.Failed;

            if (mission.FinalTarget != null)
                if (await mission.FinalTarget.WaitForTargetToBeReached())
                {
                    var finalMissionResult = DetermineMissionResult(npcId.GoodBadNpc, mission.FinalTarget.TargetType);
                    if (finalMissionResult == MissionResultType.Complete)
                        mission.MissionResult = MissionResultType.Complete;
                }

            // Отправка результата
            var npcType = npcId.GoodBadNpc;
            var finalResult = mission.MissionResult == MissionResultType.Complete
                ? npcType == GoodBadEndingType.Bad ? GoodBadEndingType.Bad : GoodBadEndingType.Good
                : npcType == GoodBadEndingType.Bad
                    ? GoodBadEndingType.Good
                    : GoodBadEndingType.Bad;

            OnMissionFinished?.Invoke(this, finalResult);
        }

        protected override UniTask OnHasNotDeal()
        {
            Debug.Log("Thank you, you did everything that I needed.");
            return UniTask.CompletedTask;
        }
    }
}