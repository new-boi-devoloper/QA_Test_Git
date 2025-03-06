using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MB.Items.Weapons.Abstract;
using MB.Items.Weapons.WeaponTypes;
using MB.Player.AnimsBehavior;
using Zenject;
using Object = UnityEngine.Object;

namespace MB.Player.PlayerFunctions
{
    public class PlayerAttacker
    {
        private readonly Dictionary<Type, IAttackStrategy> _attackStrategies = new();
        private readonly PlayerAnimator _playerAnimator;
        private readonly PlayerContainer _playerContainer;

        [Inject]
        public PlayerAttacker(PlayerContainer playerContainer, PlayerAnimator playerAnimator)
        {
            _playerContainer = playerContainer;
            _playerAnimator = playerAnimator;
        }

        public IAttackStrategy CurrentAttackStrategy { get; private set; }
        public AbstractWeapon CurrentWeapon { get; private set; }

        public void RegularAttack()
        {
            if (CurrentWeapon == null
                || CurrentAttackStrategy == null
                || _playerContainer.InteractionMode != InteractionMode.ThirdPerson) return;

            CurrentAttackStrategy.Attack(
                CurrentWeapon,
                CurrentWeapon.BaseDamage,
                CurrentWeapon.AttackSpeed);
        }

        public void SwitchWeapon(AbstractWeapon weapon)
        {
            if (weapon == null) return;

            // Удаляем старое оружие, если оно есть
            if (CurrentWeapon != null) Object.Destroy(CurrentWeapon.gameObject);

            // Создаем новое оружие и делаем его дочерним объектом
            CurrentWeapon = Object.Instantiate(weapon, _playerContainer.WeaponPoint.transform);
            // CurrentWeapon.transform.localPosition = Vector3.zero;
            // CurrentWeapon.transform.localRotation = Quaternion.identity;

            // Создаем или используем существующую стратегию атаки
            if (!_attackStrategies.TryGetValue(weapon.GetType(), out var strategy))
            {
                strategy = CreateAttackStrategy(weapon);
                _attackStrategies[weapon.GetType()] = strategy;
            }

            CurrentAttackStrategy = strategy;
        }

        private IAttackStrategy CreateAttackStrategy(AbstractWeapon weapon)
        {
            return weapon switch
            {
                Axe => new AxeAttackStrategy(_playerAnimator),
                OneHandedSword => new OneHandedSwordAttackStrategy(_playerAnimator),
                _ => throw new NotSupportedException($"Unsupported weapon type: {weapon.GetType()}")
            };
        }
    }

    public interface IAttackStrategy
    {
        void Attack(AbstractWeapon currentWeapon, float damage, float attackSpeed);
    }

    public class AxeAttackStrategy : IAttackStrategy
    {
        //dependencies
        private readonly PlayerAnimator _playerAnimator;

        public AxeAttackStrategy(PlayerAnimator playerAnimator)
        {
            _playerAnimator = playerAnimator;
        }

        public void Attack(AbstractWeapon currentWeapon, float damage, float attackSpeed)
        {
            // вызов метода проигрывания аниммации с передачей типа атаки (то есть меч или секира) вместе со скоростью ататки
            _playerAnimator.PlayAttackAnimation(this, attackSpeed);
            currentWeapon.Attack(damage, attackSpeed).Forget();
        }
    }

    public class OneHandedSwordAttackStrategy : IAttackStrategy
    {
        //dependencies
        private readonly PlayerAnimator _playerAnimator;

        public OneHandedSwordAttackStrategy(PlayerAnimator playerAnimator)
        {
            _playerAnimator = playerAnimator;
        }

        public void Attack(AbstractWeapon currentWeapon, float damage, float attackSpeed)
        {
            _playerAnimator.PlayAttackAnimation(this, attackSpeed);
            currentWeapon.Attack(damage, attackSpeed).Forget();
        }
    }
}