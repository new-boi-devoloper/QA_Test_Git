using Cysharp.Threading.Tasks;
using MB.Player.PlayerFunctions;
using UnityEngine;
using Zenject;

namespace MB.Player.AnimsBehavior
{
    public class PlayerAnimator
    {
        private readonly string[] _nonSkippableAnimations;
        private readonly PlayerContainer _playerContainer;
        private string _currentAnimation;

        private Vector3 _currentPlayerMovement;
        private bool _isAttacking; // Флаг для блокировки новой атаки
        private bool _isCurrentlyRunning;

        [Inject]
        public PlayerAnimator(PlayerContainer playerContainer)
        {
            _playerContainer = playerContainer;

            _nonSkippableAnimations = new[]
            {
                "Axe_Test_2",
                "Sword_Attack"
            };
        }

        public void ManageAnimation(Vector3 playerMovement, bool isRunning)
        {
            _currentPlayerMovement = playerMovement;
            _isCurrentlyRunning = isRunning;

            // Если текущая анимация пустая, просто выходим
            if (_currentAnimation == "")
            {
                // Обработка прыжков и приземлений
                HandleJumpAndLandAnimations(playerMovement, isRunning);

                // Обработка бега, спринта и бездействия
                HandleRunSprintIdleAnimations(playerMovement, isRunning);
            }

            // Если проигрывается не пропускаемая анимация, не меняем её
            if (IsNonSkippableAnimationPlaying()) return;

            // Обработка прыжков и приземлений
            HandleJumpAndLandAnimations(playerMovement, isRunning);

            // Обработка бега, спринта и бездействия
            HandleRunSprintIdleAnimations(playerMovement, isRunning);
        }

        public async void PlayAttackAnimation(IAttackStrategy attackStrategy, float playSpeed)
        {
            // Если игрок не в прыжке и не приземляется, и атака не уже в процессе
            if (!_playerContainer.IsJumping && _playerContainer.PlayerController.isGrounded && !_isAttacking)
            {
                _isAttacking = true; // Блокируем новую атаку

                Debug.Log("Пришла команда на проигрывание анимации");

                if (attackStrategy is AxeAttackStrategy)
                    await ChangeAnimation("Axe_Test_3", 0.01f);
                else if (attackStrategy is OneHandedSwordAttackStrategy)
                    await ChangeAnimation("Sword_Attack", 0.01f);

                _isAttacking = false; // Разблокируем атаку
            }
        }

        private void HandleJumpAndLandAnimations(Vector3 playerMovement, bool isRunning)
        {
            // Если игрок в прыжке
            if (_playerContainer.IsJumping)
            {
                if (playerMovement is { x: 0, z: 0 }) // Прыжок из состояния покоя
                    ChangeAnimation("Jump_Idle").Forget();
                else if (isRunning) // Прыжок во время спринта
                    ChangeAnimation("Jump_Sprinting").Forget();
                else // Прыжок во время бега
                    ChangeAnimation("Jump_Running").Forget();
                return;
            }

            // Если игрок приземлился
            if (_currentAnimation is "Jump_Idle" or "Jump_Running" or "Jump_Sprinting" &&
                _playerContainer.PlayerController.isGrounded)
            {
                if (playerMovement is { x: 0, z: 0 }) // Приземление в состояние покоя
                    ChangeAnimation("Land_Idle").Forget();
                else if (isRunning) // Приземление во время спринта
                    ChangeAnimation("Land_Sprinting").Forget();
                else // Приземление во время бега
                    ChangeAnimation("Land_Running").Forget();
            }
        }

        private void HandleRunSprintIdleAnimations(Vector3 playerMovement, bool isRunning)
        {
            // Если игрок не в прыжке и не приземляется
            if (!_playerContainer.IsJumping && _playerContainer.PlayerController.isGrounded)
            {
                if (isRunning && playerMovement is not { x: 0, z: 0 })
                {
                    ChangeAnimation("Sprint_F").Forget();
                }
                else if (!isRunning && playerMovement is not { x: 0, z: 0 })
                {
                    // Определяем направление движения и выбираем соответствующую анимацию
                    if (playerMovement.z < 0 && playerMovement.x < 0)
                        ChangeAnimation("Run_BckStrafeBL").Forget();
                    else if (playerMovement.z < 0 && playerMovement.x > 0)
                        ChangeAnimation("Run_BckStrafeBR").Forget();
                    else if (playerMovement.x < 0)
                        ChangeAnimation("Run_BckStrafeL").Forget();
                    else if (playerMovement.z > 0 && playerMovement.x < 0)
                        ChangeAnimation("Run_BckStrafeFL").Forget();
                    else if (playerMovement.z > 0 && playerMovement.x > 0)
                        ChangeAnimation("Run_BckStrafeFR").Forget();
                    else if (playerMovement.z < 0)
                        ChangeAnimation("Run_BckStrafeB").Forget();
                    else
                        ChangeAnimation("Run_F").Forget();
                }
                else if (!isRunning && playerMovement is { x: 0, z: 0 })
                {
                    ChangeAnimation("Idle_v1").Forget();
                }
            }
        }

        public async UniTask ChangeAnimation(string animation, float crossFade = 0.1f, float time = 0f)
        {
            if (time > 0) await UniTask.Delay((int)((time - crossFade) * 1000)); // Ждем указанное время в миллисекундах

            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _playerContainer.PlayerAnimator.CrossFade(animation, crossFade);
            }
        }

        private bool IsNonSkippableAnimationPlaying()
        {
            foreach (var nonSkippableAnimation in _nonSkippableAnimations)
                if (_currentAnimation == nonSkippableAnimation)
                    return true;
            return false;
        }
    }
}