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
            // Создаём контейнер Zenject
            container = new DiContainer();

            // Создаём объект игрока
            playerObject = new GameObject("Player");
            playerContainer = playerObject.AddComponent<PlayerContainer>();

            // Добавляем необходимые компоненты
            playerContainer.PlayerController = playerObject.AddComponent<CharacterController>();

            // Настраиваем камеру (нужна для расчёта направления движения)
            playerContainer.PlayerCamera = Camera.main;
            if (playerContainer.PlayerCamera == null)
            {
                var cameraObject = new GameObject("MainCamera");
                playerContainer.PlayerCamera = cameraObject.AddComponent<Camera>();
                cameraObject.tag = "MainCamera";
                cameraObject.transform.position = new Vector3(0, 1, -10); // Позиция камеры
                cameraObject.transform.LookAt(playerObject.transform); // Смотрим на игрока
            }

            // Создаём мок для PlayerSo
            playerContainer.PlayerSo = ScriptableObject.CreateInstance<PlayerSo>();
            playerContainer.PlayerSo.playerWalkSpeed = 5f; // Скорость ходьбы
            playerContainer.PlayerSo.playerSprintSpeed = 10f; // Скорость бега
            playerContainer.PlayerSo.rotationSpeed = 10f; // Скорость поворота

            // Привязываем зависимости через Zenject
            container.Bind<PlayerContainer>().FromInstance(playerContainer).AsSingle();
            container.Bind<PlayerMovement>().AsSingle();

            // Инициализируем PlayerMovement
            playerMovement = container.Resolve<PlayerMovement>();
        }

        [TearDown]
        public void TearDown()
        {
            // Уничтожаем объекты после теста
            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(playerContainer.PlayerCamera.gameObject);
        }

        // Обычный тест (можно оставить как пример)
        [Test]
        public void MovemenebtTestSimplePasses()
        {
            Assert.IsNotNull(playerMovement, "PlayerMovement должен быть инициализирован.");
        }

        // Play Mode тест для проверки движения
        [UnityTest]
        public IEnumerator MovemenebtTestWithEnumeratorPasses()
        {
            // Задаём входные данные: движение вперёд
            var moveInput = new Vector2(0, 1); // Движение вперёд по оси Z
            var isRunPressed = false; // Ходьба, а не бег

            // Запоминаем начальную позицию
            var initialPosition = playerContainer.transform.position;

            // Выполняем метод Move
            playerMovement.Move(moveInput, isRunPressed);

            // Ждём один кадр, чтобы обработалось движение
            yield return null;

            // Получаем новую позицию
            var newPosition = playerContainer.transform.position;

            // Проверяем, что персонаж сдвинулся вперёд (по оси Z)
            Assert.Greater(newPosition.z, initialPosition.z,
                "Персонаж должен двигаться вперёд при положительном вводе по Y.");

            // Дополнительно проверяем, что движение только по оси Z (X не изменился)
            Assert.AreEqual(newPosition.x, initialPosition.x, 0.01f,
                "Персонаж не должен двигаться по оси X при вводе только по Y.");
        }


    }
}
