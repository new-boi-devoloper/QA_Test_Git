
using Cysharp.Threading.Tasks;
using MB.Npc.NpcMain.NpcVariants;
using UnityEngine;

namespace MB.Items.Abstract
{
    public abstract class AbstractItem : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public InventoryItemType InventoryItemType { get; private set; }
        [field: SerializeField] public InventoryItemRarityType InventoryItemRarityType { get; private set; }

        public bool IsInteracting { get; set; }

        public void ShowInteractivity()
        {
            Debug.Log($"Pick up {gameObject.name}");
        }

        public UniTask Interact()
        {
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }
    }

    public enum InventoryItemType
    {
        Weapon,
        Consumable
    }

    public enum InventoryItemRarityType
    {
        Mythic,
        Legendary,
        Epic,
        Rare,
        Uncommon,
        Common
    }
}