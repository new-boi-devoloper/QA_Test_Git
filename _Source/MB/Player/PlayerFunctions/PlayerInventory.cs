
using MB.Items.Weapons.Abstract;
using MB.Npc.NpcMain.NpcVariants;
using MB.SO.InventorySo;
using Zenject;

namespace MB.Player.PlayerFunctions
{
    public interface IPlayerInventory
    {
        void PickUpItem(IInteractable itemToPickUp);
        void FastWeaponSwitch();
        InventorySo GetInventory();
    }

    public class PlayerInventory : IPlayerInventory
    {
        private readonly PlayerAttacker _playerAttacker;
        private readonly PlayerContainer _playerContainer;

        private int _currentWeaponIndex = -1; // Индекс текущего оружия

        [Inject]
        public PlayerInventory(PlayerContainer playerContainer, PlayerAttacker playerAttacker)
        {
            _playerContainer = playerContainer;
            _playerAttacker = playerAttacker;

            Inventory = _playerContainer.PlayerInventory;
        }

        public InventorySo Inventory { get; }

        public void PickUpItem(IInteractable itemToPickUp)
        {
            if (itemToPickUp is AbstractWeapon weapon)
            {
                Inventory.InventoryItems.Add(new InventoryItem
                {
                    WeaponPrefab = weapon,
                    Amount = 1
                });

                // Если это первое оружие, автоматически выбираем его
                if (_currentWeaponIndex == -1)
                {
                    _currentWeaponIndex = 0;
                    _playerAttacker.SwitchWeapon(weapon);
                }
            }
        }

        public InventorySo GetInventory()
        {
            return Inventory;
        }

        public void FastWeaponSwitch()
        {
            if (Inventory.InventoryItems.Count == 0) return;

            // Переключаемся на следующее оружие
            _currentWeaponIndex = (_currentWeaponIndex + 1) % Inventory.InventoryItems.Count;
            var nextWeapon = Inventory.InventoryItems[_currentWeaponIndex].WeaponPrefab;

            _playerAttacker.SwitchWeapon(nextWeapon);
        }
    }
}