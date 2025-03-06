using MB.Player.Abstract;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace MB.MySystem
{
    public class InputListener : MonoBehaviour
    {
        private Vector2 _currentMovementInput; // Теперь используем Vector2
        private bool _isEKeyPressed;
        private bool _isJumpPressed;
        private bool _isMovementPressed;
        private bool _isRunPressed;

        private MyPlayerInput _playerInput;
        private IPlayerInvoker _playerInvoker;

        private void Update()
        {
            _playerInvoker.OnPlayerInput(_currentMovementInput, _isRunPressed, _isJumpPressed);
        }

        private void OnEnable()
        {
            _playerInput.Player.Enable();

            _playerInput.Player.Move.started += OnMovementInput;
            _playerInput.Player.Move.canceled += OnMovementInput;
            _playerInput.Player.Move.performed += OnMovementInput;

            _playerInput.Player.Sprint.performed += OnRun;
            _playerInput.Player.Sprint.canceled += OnRun;

            _playerInput.Player.Jump.started += OnJump;
            _playerInput.Player.Jump.canceled += OnJump;

            _playerInput.Player.Interact.started += OnInteract;
            _playerInput.Player.SwitchInteraction.started += OnSwitchInteraction;
            _playerInput.Player.Attack.started += OnAttack;
            _playerInput.Player.FastSwitchWeapon.started += OnFastSwitchWeapon;
        }

        private void OnDisable()
        {
            _playerInput.Player.Move.started -= OnMovementInput;
            _playerInput.Player.Move.canceled -= OnMovementInput;
            _playerInput.Player.Move.performed -= OnMovementInput;

            _playerInput.Player.Sprint.performed -= OnRun;
            _playerInput.Player.Sprint.canceled -= OnRun;

            _playerInput.Player.Jump.started -= OnJump;
            _playerInput.Player.Jump.canceled -= OnJump;

            _playerInput.Player.Interact.started -= OnInteract;
            _playerInput.Player.SwitchInteraction.started -= OnSwitchInteraction;
            _playerInput.Player.Attack.started -= OnAttack;
            _playerInput.Player.FastSwitchWeapon.started -= OnFastSwitchWeapon;

            _playerInput.Player.Disable();
        }

        private void OnFastSwitchWeapon(InputAction.CallbackContext obj)
        {
            _playerInvoker.OnFastSwitchWeapon();
        }

        private void OnAttack(InputAction.CallbackContext obj)
        {
            _playerInvoker.OnAttack();
        }

        [Inject]
        public void Construct(IPlayerInvoker playerInvoker, MyPlayerInput playerInput)
        {
            _playerInvoker = playerInvoker;
            _playerInput = playerInput;
        }

        private void OnSwitchInteraction(InputAction.CallbackContext obj)
        {
            _playerInvoker.OnSwitchInteraction();
        }

        private void OnInteract(InputAction.CallbackContext obj)
        {
            _playerInvoker.OnInteract();
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
        }

        private void OnRun(InputAction.CallbackContext obj)
        {
            _isRunPressed = obj.ReadValueAsButton();
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            _isJumpPressed = obj.ReadValueAsButton();
        }
    }
}