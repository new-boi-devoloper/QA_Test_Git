using MB.ArmyManagement;
using MB.MySystem;
using MB.Player;
using MB.Player.Abstract;
using MB.Player.AnimsBehavior;
using MB.Player.PlayerFunctions;
using MB.QuestLogic;
using MB.SO.NpcSos.Main;
using MB.UI.DialogUI;
using UnityEngine;
using Zenject;

namespace MB.Main
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private InputListener inputListener;
        [SerializeField] private PlayerContainer playerContainer;
        [SerializeField] private PlayerInteractor playerInteractor;
        [SerializeField] private CameraSwitcher cameraSwitcher;

        [Header("All Npc Data")] [SerializeField]
        private NpcSetupSo npcSetupSo;

        [Header("UI dependencies")] [SerializeField]
        private DialogView dialogView;

        public override void InstallBindings()
        {
            // Player
            Container.Bind<InputListener>().FromInstance(inputListener).AsSingle().Lazy();
            Container.Bind<PlayerContainer>().FromInstance(playerContainer).AsSingle().Lazy();
            Container.Bind<IPlayerInteractor>().To<PlayerInteractor>().FromInstance(playerInteractor).AsTransient()
                .Lazy();
            Container.Bind<IArmyChoser>().To<PlayerInteractor>().FromInstance(playerInteractor).AsTransient().Lazy();
            Container.Bind<CameraSwitcher>().FromInstance(cameraSwitcher).AsSingle().Lazy();

            Container.Bind<NpcSetupSo>().FromInstance(npcSetupSo).AsSingle().Lazy();


            Container.Bind<IPlayerInvoker>().To<PlayerInvoker>().AsSingle().NonLazy();
            Container.Bind<PlayerMovement>().AsSingle().Lazy();
            Container.Bind<PlayerAnimator>().AsSingle().Lazy();
            Container.Bind<MyPlayerInput>().AsSingle().Lazy();
            Container.Bind<IUnitSelectionner>().To<UnitSelectionner>().AsSingle().Lazy();

            Container.Bind<PlayerAttacker>().AsSingle().Lazy();
            Container.Bind<IPlayerInventory>().To<PlayerInventory>().AsSingle().Lazy();

            Container.Bind<GameProgressTracker>().AsSingle().Lazy();

            //UI
            Container.Bind<DialogView>().FromInstance(dialogView).AsSingle().Lazy();
            Container.Bind<DialogController>().AsSingle().Lazy();
        }
    }
}