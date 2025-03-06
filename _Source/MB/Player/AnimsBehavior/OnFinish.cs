using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MB.Player.AnimsBehavior
{
    public class OnFinish : StateMachineBehaviour
    {
        [SerializeField] private string animation;

        [Inject] private PlayerAnimator _playerAnimator;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerAnimator.ChangeAnimation(animation, 0.2f, stateInfo.length).Forget();
        }
    }
}