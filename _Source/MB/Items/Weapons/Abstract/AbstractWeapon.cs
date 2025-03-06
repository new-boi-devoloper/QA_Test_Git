using Cysharp.Threading.Tasks;
using MB.Items.Abstract;
using UnityEngine;

namespace MB.Items.Weapons.Abstract
{
    [RequireComponent(typeof(MeshCollider))]
    public abstract class AbstractWeapon : AbstractItem
    {
        [field: SerializeField] public float BaseDamage { get; private set; }
        [field: SerializeField] public float AttackSpeed { get; private set; }

        protected abstract LayerMask EnemyLayer { get; set; }
        protected abstract MeshCollider WeaponCollider { get; set; }
        protected float CurrentDamage { get; private set; }

        public async UniTask Attack(float attackDamage, float attackSpeed)
        {
            CurrentDamage = attackDamage;

            // // Включаем коллайдер на время атаки
            // if (WeaponCollider != null)
            // {
            //     WeaponCollider.enabled = true;
            // }

            await UniTask.Delay((int)(attackSpeed * 1000)); // Ждем указанное время в миллисекундах

            // if (WeaponCollider != null)
            // {
            //     WeaponCollider.enabled = false;
            // }
        }
    }
}