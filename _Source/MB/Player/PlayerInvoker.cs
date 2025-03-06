using MB.Player.Abstract;
using MB.Player.AnimsBehavior;
using MB.Player.PlayerFunctions;
using UnityEngine;
using Zenject;

namespace MB.Player
{
    public class PlayerInvoker : IPlayerInvoker
    {
        private readonly PlayerAnimator _playerAnimator;
        private readonly PlayerAttacker _playerAttacker;
        private readonly PlayerContainer _playerContainer;
        private readonly IPlayerInteractor _playerInteractor;
        private readonly IPlayerInventory _playerInventory;
        private readonly PlayerMovement _playerMovement;

        [Inject]
        public PlayerInvoker(
            PlayerMovement playerMovement,
            PlayerContainer playerContainer,
            PlayerAnimator playerAnimator,
            IPlayerInteractor playerInteractor,
            PlayerAttacker playerAttacker,
            IPlayerInventory playerInventory)
        {
            _playerMovement = playerMovement;
            _playerContainer = playerContainer;
            _playerAnimator = playerAnimator;
            _playerInteractor = playerInteractor;
            _playerAttacker = playerAttacker;
            _playerInventory = playerInventory;
        }

        public void OnPlayerInput(Vector2 moveInput, bool isRunPressed, bool isJumpPressed)
        {
            _playerMovement.Move(moveInput, isRunPressed);
            _playerMovement.Jump(isJumpPressed);
            _playerAnimator.ManageAnimation(new Vector3(moveInput.x, 0, moveInput.y), isRunPressed);
        }

        public void OnInteract()
        {
            _playerInteractor.Interact();
        }

        public void OnSwitchInteraction()
        {
            _playerInteractor.SwitchInteractionMode();
        }

        public void OnAttack()
        {
            _playerAttacker.RegularAttack();
        }

        public void OnFastSwitchWeapon()
        {
            _playerInventory.FastWeaponSwitch();
        }
    }
}