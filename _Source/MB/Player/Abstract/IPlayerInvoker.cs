using UnityEngine;

namespace MB.Player.Abstract
{
    public interface IPlayerInvoker
    {
        void OnPlayerInput(Vector2 moveInput, bool isRunPressed, bool isJumpPressed);
        void OnInteract();
        void OnSwitchInteraction();
        void OnAttack();
        void OnFastSwitchWeapon();
    }
}