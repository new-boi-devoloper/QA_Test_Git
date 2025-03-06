using MB.Items.Weapons.Abstract;
using UnityEngine;

namespace MB.Items.Weapons.WeaponTypes
{
    public class OneHandedSword : AbstractWeapon
    {
        [field: SerializeField] protected override LayerMask EnemyLayer { get; set; }
        protected override MeshCollider WeaponCollider { get; set; }

        private void OnEnable()
        {
            WeaponCollider = GetComponent<MeshCollider>();
            // WeaponCollider.enabled = false;

            Debug.Log($"enemy layer: {EnemyLayer.value} and {WeaponCollider.name}");
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log($"collided with {other.gameObject.name}");
            if ((EnemyLayer & (1 << other.gameObject.layer)) != 0)
                Debug.Log($"provided damage {CurrentDamage} for {other.gameObject.name}");
            // Здесь можно добавить логику нанесения урона
        }
    }
}