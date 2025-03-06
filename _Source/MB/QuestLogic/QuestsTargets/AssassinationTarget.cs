using Cysharp.Threading.Tasks;
using MB.Npc.NpcMain;
using MB.QuestLogic.QuestsTargets.Abstract;
using UnityEngine;

namespace MB.QuestLogic.QuestsTargets
{
    public class AssassinationTarget : QuestTarget
    {
        // Тип цели (Good или Bad)
        [field: SerializeField] public override GoodBadEndingType TargetType { get; protected set; }
        private NpcHealth _npcHealth;

        public override async UniTask<bool> WaitForTargetToBeReached()
        {
            IsTargetAchieved = false;

            if (gameObject.TryGetComponent(out NpcHealth npcHealth))
            {
                _npcHealth = npcHealth;
                _npcHealth.OnDead += GoalAchieved;
            }

            return await base.WaitForTargetToBeReached();
        }
    }
}