using System;
using Dots.Authoring;
using MB.ArmyManagement;
using MB.Items.Abstract;
using MB.MySystem;
using MB.Npc.NpcMain.NpcVariants;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using Zenject;
using Plane = UnityEngine.Plane;
using RaycastHit = UnityEngine.RaycastHit;

namespace MB.Player.PlayerFunctions
{
    public interface IPlayerInteractor
    {
        void SwitchInteractionMode();
        void Interact();
    }

    public interface IArmyChoser
    {
        event EventHandler OnSelectionAreaStart;
        event EventHandler OnSelectionAreaEnd;
        Rect GetSelectionAreaRect();
    }

    public class PlayerInteractor : MonoBehaviour, IPlayerInteractor, IArmyChoser
    {
        // Настройки
        [SerializeField] private float thirdPersonInteractionDistance = 5f;
        [SerializeField] private float sphereCastRadius = 1f;
        [SerializeField] private float RTSMaxDistance = 100f;
        [SerializeField] private LayerMask interactionLayer;
        [SerializeField] private Vector3 rayOffset;
        private readonly RaycastHit[] _hits = new RaycastHit[10];

        // Зависимости
        private CameraSwitcher _cameraSwitcher;
        private IInteractable _currentInteractable;
        private Camera _mainCamera;
        private PlayerContainer _playerContainer;
        private IPlayerInventory _playerInventory;

        // Логика
        // private InteractionMode _currentMode = InteractionMode.ThirdPerson;
        private Vector2 _selectionStartMousePosition;
        private IUnitSelectionner _unitSelectionner;

        private void Start()
        {
            _mainCamera = Camera.main;
            _cameraSwitcher.SwitchToThirdPersonCamera(); // Устанавливаем начальную камеру

            if (_cameraSwitcher == null) Debug.Log("оно нал");
        }

        private void Update()
        {
            if (_playerContainer.InteractionMode == InteractionMode.NoInteraction) return;

            if (_playerContainer.InteractionMode == InteractionMode.ThirdPerson)
                HandleThirdPersonInteraction();
            else if (_playerContainer.InteractionMode == InteractionMode.RTS) HandleRtsInteraction();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_mainCamera != null)
            {
                var rayDirection =
                    _mainCamera.transform.forward + _mainCamera.transform.TransformDirection(rayOffset);
                Gizmos.DrawRay(_mainCamera.transform.position, rayDirection * 20);
            }
        }
#endif

        public event EventHandler OnSelectionAreaStart;
        public event EventHandler OnSelectionAreaEnd;

        public Rect GetSelectionAreaRect()
        {
            Vector2 selectionEndMousePosition = Input.mousePosition;

            var lowerLeftCorner = new Vector2
            (
                Mathf.Min(_selectionStartMousePosition.x, selectionEndMousePosition.x),
                Mathf.Min(_selectionStartMousePosition.y, selectionEndMousePosition.y)
            );

            var upperRightCorner = new Vector2
            (
                Mathf.Max(_selectionStartMousePosition.x, selectionEndMousePosition.x),
                Mathf.Max(_selectionStartMousePosition.y, selectionEndMousePosition.y)
            );

            return new Rect(
                lowerLeftCorner.x,
                lowerLeftCorner.y,
                upperRightCorner.x - lowerLeftCorner.x,
                upperRightCorner.y - lowerLeftCorner.y
            );
        } // TODO: вынсти в отдельный класс

        public async void Interact()
        {
            if (_playerContainer.InteractionMode == InteractionMode.ThirdPerson)
            {
                if (_currentInteractable is InteractNpcBase npc)
                {
                    // Проверяем, не взаимодействует ли NPC уже
                    if (!npc.IsInteracting)
                    {
                        _playerContainer.InteractionMode = InteractionMode.NoInteraction;
                        _cameraSwitcher.SwitchToDialogCamera();

                        Debug.Log($"Начало взаимодействия: {npc.IsInteracting}");
                        await npc.Interact(); // Ожидаем завершения диалога
                        Debug.Log($"Завершение взаимодействия: {npc.IsInteracting}");

                        // После завершения диалога переключаемся обратно на ThirdPerson
                        _playerContainer.InteractionMode = InteractionMode.ThirdPerson;
                        _cameraSwitcher.SwitchToThirdPersonCamera();
                    }
                }
                else if (_currentInteractable is AbstractItem item)
                {
                    _playerInventory.PickUpItem(item);
                    await item.Interact(); // Ожидаем завершения взаимодействия с предметом
                }
            }
        }

        public void SwitchInteractionMode()
        {
            if (_playerContainer.InteractionMode == InteractionMode.NoInteraction) return;

            _playerContainer.InteractionMode = _playerContainer.InteractionMode == InteractionMode.ThirdPerson
                ? InteractionMode.RTS
                : InteractionMode.ThirdPerson;

            if (_playerContainer.InteractionMode == InteractionMode.ThirdPerson)
                _cameraSwitcher.SwitchToThirdPersonCamera();
            else if (_playerContainer.InteractionMode == InteractionMode.RTS)
                _cameraSwitcher.SwitchToRtsCamera();
        }

        [Inject]
        public void Construct(
            CameraSwitcher cameraSwitcher,
            IUnitSelectionner unitSelectionner,
            IPlayerInventory playerInventory,
            PlayerContainer playerContainer)
        {
            _cameraSwitcher = cameraSwitcher;
            _unitSelectionner = unitSelectionner;
            _playerInventory = playerInventory;
            _playerContainer = playerContainer;
        }


        private void HandleThirdPersonInteraction()
        {
            var rayOrigin = _mainCamera.transform.position;
            var rayDirection = _mainCamera.transform.forward +
                               _mainCamera.transform.TransformDirection(rayOffset).normalized;

            var hitCount = Physics.SphereCastNonAlloc(rayOrigin, sphereCastRadius, rayDirection, _hits,
                thirdPersonInteractionDistance, interactionLayer);

            _currentInteractable = null;

            for (var i = 0; i < hitCount; i++)
                if (_hits[i].collider.TryGetComponent<IInteractable>(out var interactable) &&
                    !interactable.IsInteracting)
                {
                    _currentInteractable = interactable;
                    _currentInteractable.ShowInteractivity();
                    break;
                }
        }

        private void HandleRtsInteraction()
        {
            if (Input.GetMouseButtonDown(1)) // ЛКМ вниз для выбора позиции начала зоны выбора юнитов
            {
                _selectionStartMousePosition = Input.mousePosition;
                OnSelectionAreaStart?.Invoke(this, EventArgs.Empty);
            }

            if (Input.GetMouseButtonUp(1)) // ЛКМ вверх для выбора позиции конца зоны выбора юнитов
            {
                Vector2 selectionEndMousePosition;
                selectionEndMousePosition = Input.mousePosition;
                OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);

                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


                //Сначала выключить Selected на false у всех юнитов перед новым выбором

                var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithPresent<Selected>().Build(entityManager);
                var selectedEntities = entityQuery.ToEntityArray(Allocator.Temp);
                for (var i = 0; i < selectedEntities.Length; i++)
                    entityManager.SetComponentEnabled<Selected>(selectedEntities[i], false);

                // проверка на то много ли юнитов хочет выбрать игрок или нет

                var selectionAreaRect = GetSelectionAreaRect();
                var selectionAreaSize = selectionAreaRect.width + selectionAreaRect.height;
                var multipleSelectionSizeMin = 50f;
                var multipleSelect = selectionAreaSize > multipleSelectionSizeMin;

                // А теперь у всех юнитов что соответсвуют включить Selected на true

                if (multipleSelect)
                {
                    entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Unit>()
                        .WithPresent<Selected>().Build(entityManager);

                    var entityArray = entityQuery.ToEntityArray(Allocator.Temp);

                    var localTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

                    for (var i = 0; i < localTransformArray.Length; i++)
                    {
                        var unitLocalTransform = localTransformArray[i];
                        var unitScreenPosition = Camera.main.WorldToScreenPoint(unitLocalTransform.Position);
                        if (selectionAreaRect.Contains(unitScreenPosition))
                            // юнит внутри зоны выбора
                            entityManager.SetComponentEnabled<Selected>(entityArray[i], true);
                    }
                }
                else
                {
                    entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                    var physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
                    var collisionWorld = physicsWorldSingleton.CollisionWorld;

                    var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var unitsLayer = 10;
                    var raycastInput = new RaycastInput
                    {
                        Start = cameraRay.GetPoint(0f),
                        End = cameraRay.GetPoint(9999f),
                        Filter = new CollisionFilter
                        {
                            BelongsTo = ~0u,
                            CollidesWith = 1u << unitsLayer,
                            GroupIndex = 0
                        }
                    };
                    if (collisionWorld.CastRay(raycastInput, out var raycastHit))
                        if (entityManager.HasComponent<Unit>(raycastHit.Entity))
                            entityManager.SetComponentEnabled<Selected>(raycastHit.Entity, true);
                }
            }

            if (Input.GetMouseButtonDown(0)) // ЛКМ для выбора позиции
            {
                var mouseWorldPosition = GetMouseWorldPosition();
                if (mouseWorldPosition != Vector3.zero) _unitSelectionner.SelectUnits(mouseWorldPosition);
            }
        }


        private Vector3 GetMouseWorldPosition()
        {
            var mouseCameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(mouseCameraRay, out var distance)) return mouseCameraRay.GetPoint(distance);

            Debug.Log("Can't read mouse position in the world");
            return Vector3.zero;
        } // TODO: вынсти в отдельный класс
    }


    public enum InteractionMode
    {
        ThirdPerson,
        RTS,
        NoInteraction
    }
}