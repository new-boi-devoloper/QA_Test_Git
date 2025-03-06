using System.Collections;
using MB.Player;
using MB.Player.PlayerFunctions;
using MB.SO.PlayerSo;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.MyTestAssembly
{
    public class MoveTest
    {
        private DiContainer container;
        private PlayerContainer playerContainer;
        private PlayerMovement playerMovement;
        private GameObject playerObject;

        [SetUp]
        public void Setup()
        {
            container = new DiContainer();

            playerObject = new GameObject("Player");
            playerContainer = playerObject.AddComponent<PlayerContainer>();

            playerContainer.PlayerController = playerObject.AddComponent<CharacterController>();

            playerContainer.PlayerCamera = Camera.main;
            if (playerContainer.PlayerCamera == null)
            {
                var cameraObject = new GameObject("MainCamera");
                playerContainer.PlayerCamera = cameraObject.AddComponent<Camera>();
                cameraObject.tag = "MainCamera";
                cameraObject.transform.position = new Vector3(0, 1, -10);
                cameraObject.transform.LookAt(playerObject.transform);
            }

            playerContainer.PlayerSo = ScriptableObject.CreateInstance<PlayerSo>();
            playerContainer.PlayerSo.playerWalkSpeed = 5f; 
            playerContainer.PlayerSo.playerSprintSpeed = 10f; 
            playerContainer.PlayerSo.rotationSpeed = 10f;

            container.Bind<PlayerContainer>().FromInstance(playerContainer).AsSingle();
            container.Bind<PlayerMovement>().AsSingle();

            playerMovement = container.Resolve<PlayerMovement>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(playerContainer.PlayerCamera.gameObject);
        }

        [Test]
        public void MovemenebtTestSimplePasses()
        {
            Assert.IsNotNull(playerMovement, "PlayerMovement должен быть инициализирован.");
        }

        [UnityTest]
        public IEnumerator MovemenebtTestWithEnumeratorPasses()
        {
            var moveInput = new Vector2(0, 1); 
            var isRunPressed = false; 

            var initialPosition = playerContainer.transform.position;

            playerMovement.Move(moveInput, isRunPressed);

            yield return null;

            var newPosition = playerContainer.transform.position;

            Assert.Greater(newPosition.z, initialPosition.z,
                "Персонаж должен двигаться вперёд при положительном вводе по Y.");

            Assert.AreEqual(newPosition.x, initialPosition.x, 0.01f,
                "Персонаж не должен двигаться по оси X при вводе только по Y.");
        }


    }
}
