using System;
using System.Collections.Generic;
using MB.Items.Weapons.Abstract;
using UnityEngine;

namespace MB.SO.InventorySo
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Data/ InventoryData", order = 0)]
    public class InventorySo : ScriptableObject
    {
        public List<InventoryItem> InventoryItems = new();
    }

    [Serializable]
    public struct InventoryItem
    {
        public AbstractWeapon WeaponPrefab; // Префаб оружия
        public int Amount; // Количество (если нужно)
    }
}