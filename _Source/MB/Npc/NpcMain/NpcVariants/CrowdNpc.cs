using UnityEngine;
using UnityEngine.AI;

namespace MB.Npc.NpcMain.NpcVariants
{
    public class CrowdNpc : Npc
    {
        public override CapsuleCollider NpcCollider { get; protected set; }
        public override NavMeshAgent Agent { get; protected set; }

        private void Start()
        {
            NpcCollider = GetComponent<CapsuleCollider>();
            Agent = GetComponent<NavMeshAgent>();
        }
    }
}