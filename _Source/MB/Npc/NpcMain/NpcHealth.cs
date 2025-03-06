using System;
using MB.SO.NpcSos.Main;
using UnityEngine;

namespace MB.Npc.NpcMain
{
    public class NpcHealth : MonoBehaviour
    {
        [field: SerializeField] public NpcPhysicalSo NpcPhysicalSo { get; private set; }
        private int _currentHealth;

        private void Start()
        {
            _currentHealth = NpcPhysicalSo.baseHealth;
        }

        public event Action OnDead;

        public bool TryChangeHealth(int value)
        {
            if (value <= 0)
            {
                _currentHealth += value;
                Debug.Log($"{gameObject.name} got hit with {value}");
                if (_currentHealth <= 0) OnDead?.Invoke();

                return true;
            }

            if (value >= 0)
            {
                _currentHealth += value;
                Debug.Log($"{gameObject.name} got healed with {value}");
                return true;
            }

            return false;
        }
    }
}