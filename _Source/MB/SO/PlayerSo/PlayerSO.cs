using UnityEngine;

namespace MB.SO.PlayerSo
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/ PlayerData", order = 0)]
    public class PlayerSo : ScriptableObject
    {
        public float playerWalkSpeed;
        public float playerSprintSpeed;
        public float jumpPower;
        public float rotationSpeed;
    }
}