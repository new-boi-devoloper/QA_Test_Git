using MB.Player.PlayerFunctions;
using MB.SO.InventorySo;
using MB.SO.PlayerSo;
using UnityEngine;

namespace MB.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerContainer : MonoBehaviour
    {
        // Input Data
        [field: SerializeField] public PlayerSo PlayerSo { get; set; }
        [field: SerializeField] public InventorySo PlayerInventory { get; private set; }
        [field: SerializeField] public GameObject WeaponPoint { get; private set; }

        // Player Data
        public bool IsJumping { get; set; }
        public bool IsAttacking { get; set; }
        public InteractionMode InteractionMode { get; set; }
        public Vector3 PlayerCurrentPosition { get; private set; }

        // Components
        public CharacterController PlayerController { get; set; }
        public Animator PlayerAnimator { get; private set; }
        public Camera PlayerCamera { get; set; } // Ссылка на камеру

        private void Start()
        {
            PlayerController = GetComponent<CharacterController>();
            PlayerAnimator = GetComponent<Animator>();
            PlayerCamera = Camera.main;
        }

        private void Update()
        {
            PlayerCurrentPosition = transform.position;
        }
    }
}