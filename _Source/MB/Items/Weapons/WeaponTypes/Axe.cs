using MB.Items.Weapons.Abstract;
using UnityEngine;

namespace MB.Items.Weapons.WeaponTypes
{
    public class Axe : AbstractWeapon
    {
        [field: SerializeField] protected override LayerMask EnemyLayer { get; set; }
        protected override MeshCollider WeaponCollider { get; set; }

        private void OnEnable()
        {
            WeaponCollider = GetComponent<MeshCollider>();
            WeaponCollider.enabled = false;

            Debug.Log($"enemy layer: {EnemyLayer} and {WeaponCollider.name}");
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"collided with {other.gameObject.name}");
            if ((EnemyLayer & (1 << other.gameObject.layer)) != 0)
                Debug.Log($"provided damage {CurrentDamage} for {other.gameObject.name}");
            // Здесь можно добавить логику нанесения урона
        }
    }
}