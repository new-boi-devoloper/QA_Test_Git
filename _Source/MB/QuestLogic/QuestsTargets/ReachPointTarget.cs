using Cysharp.Threading.Tasks;
using MB.QuestLogic.QuestsTargets.Abstract;
using UnityEngine;

namespace MB.QuestLogic.QuestsTargets
{
    public class ReachPointTarget : QuestTarget
    {
        // Тип цели (Good или Bad)
        [field: SerializeField] public override GoodBadEndingType TargetType { get; protected set; }

        //
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) GoalAchieved();
        }

        public override async UniTask<bool> WaitForTargetToBeReached()
        {
            return await base.WaitForTargetToBeReached();
        }
    }
}