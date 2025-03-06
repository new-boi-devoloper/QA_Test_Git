using Cysharp.Threading.Tasks;
using MB.QuestLogic;
using UnityEngine;

namespace MB.Npc.NpcMain.NpcVariants
{
    public interface IInteractable
    {
        public bool IsInteracting { get; set; }
        void ShowInteractivity();
        UniTask Interact();
    }

    public abstract class InteractNpcBase : Npc, IInteractable
    {
        protected bool AbleToTalk { get; set; } = true;
        public bool IsInteracting { get; set; }

        public void ShowInteractivity()
        {
            Debug.Log($"ShowInteractivity in {gameObject.name}");
        }

        public async UniTask Interact()
        {
            if (IsInteracting) return;
            Debug.Log($"1{IsInteracting}");
            IsInteracting = true;

            if (AbleToTalk)
                await OnHasDeal();
            else
                await OnHasNotDeal();

            Debug.Log($"2{IsInteracting}");
            IsInteracting = false;
        }

        // для торговца если можно с ним торговать, для выдавателя миссий если они ещё есть и нпс хочет с игроком говорить
        protected abstract UniTask OnHasDeal();

        // если нпс не хочет начинать диалог
        protected abstract UniTask OnHasNotDeal();

        protected MissionResultType DetermineMissionResult(GoodBadEndingType npcType, GoodBadEndingType targetType)
        {
            if (targetType == GoodBadEndingType.Neutral)
                return MissionResultType.Neutral; // Нейтральная цель
            if (npcType == targetType)
                return MissionResultType.Complete; // Цель соответствует типу NPC
            return MissionResultType.OppositeComplete; // Цель противоположна типу NPC
        }
    }
}