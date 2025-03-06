namespace MB.Npc.NpcMain
{
    public class NpcLogister
    {
        // private readonly NpcSetupSo _npcSetup;
        // private readonly EnemySetupSo _enemySetup;
        // private readonly PlayerContainer _playerContainer;
        // private readonly GameSettingsSo _gameSettingsSo;
        //
        // [Inject]
        // public NpcLogister(
        //     NpcSetupSo npcSetup,
        //     PlayerContainer playerContainer,
        //     GameSettingsSo gameSettingsSo,
        //     EnemySetupSo enemySetup)
        // {
        //     _npcSetup = npcSetup;
        //     _enemySetup = enemySetup;
        //     _playerContainer = playerContainer;
        //     _gameSettingsSo = gameSettingsSo;
        //
        //     _playerContainer.OnChangePosition += ManageNpcs;
        // }
        //
        // public void InitNpcs()
        // {
        //     foreach (var npc in _npcSetup.setSideQuestNpcs)
        //     {
        //         npc.Npc.gameObject.SetActive(false);
        //     }
        //
        //     // позже
        //     foreach (var enemy in _enemySetup.enemySetupData)
        //     {
        //         //  enemy.Enemy.gameObject.SetActive(false);
        //     }
        // }
        //
        // private void ManageNpcs(Vector3 newPlayerPosition)
        // {
        //     foreach (var npcSetup in _npcSetup.setSideQuestNpcs)
        //     {
        //         if (Vector3.SqrMagnitude(newPlayerPosition - npcSetup.Npc.transform.position) <=
        //             _gameSettingsSo.npcTurnOnDistance)
        //         {
        //             npcSetup.Npc.gameObject.SetActive(true);
        //         }
        //         else
        //         {
        //             npcSetup.Npc.gameObject.SetActive(false);
        //         }
        //     }
        // }
        //
        // private void ManageEnemies(Vector3 newPlayerPosition)
        // {
        //     foreach (var npcSetup in _enemySetup.enemySetupData)
        //     {
        //         // if (Vector3.Magnitude(newPlayerPosition)
        //         //     <= Vector3.Magnitude(npcSetup.Npc.transform.position) + _gameSettingsSo.enemyTurnOnDistance)
        //         // {
        //         //     npcSetup.Npc.gameObject.SetActive(true);
        //         // }
        //         // else
        //         // {
        //         //     npcSetup.Npc.gameObject.SetActive(false);
        //         // }
        //     }
        // }
    }
}