using UnityEngine;
using UnityEngine.AI;

namespace MB.Npc.NpcMain
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CapsuleCollider))]
    public abstract class Npc : MonoBehaviour
    {
        public abstract CapsuleCollider NpcCollider { get; protected set; }
        public abstract NavMeshAgent Agent { get; protected set; }
    
    }
}