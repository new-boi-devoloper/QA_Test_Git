using MB.Npc.NpcMain;
using UnityEngine;
using Zenject;

namespace MB.Main
{
    public class GameManager : MonoBehaviour
    {
        private NpcLogister _npcLogister;

        private void Start()
        {
        }

        [Inject]
        public void Construct(NpcLogister npcLogister)
        {
            _npcLogister = npcLogister;
        }
    }
}