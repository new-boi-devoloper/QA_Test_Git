using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MB.QuestLogic.QuestsTargets.Abstract
{
    public abstract class QuestTarget : MonoBehaviour
    {
        private UniTaskCompletionSource<bool> _completionSource;
        public abstract GoodBadEndingType TargetType { get; protected set; } // Тип цели (Good или Bad)
        protected bool IsTargetAchieved { get; set; }

        public virtual async UniTask<bool> WaitForTargetToBeReached()
        {
            _completionSource = new UniTaskCompletionSource<bool>();
            return await _completionSource.Task;
        }

        private void CompleteGoal(bool success)
        {
            _completionSource?.TrySetResult(success);
        }

        protected virtual void GoalAchieved()
        {
            IsTargetAchieved = true;
            CompleteGoal(IsTargetAchieved);
        }
    }
}