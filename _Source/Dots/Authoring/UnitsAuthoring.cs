using Unity.Entities;
using UnityEngine;

namespace Dots.Authoring
{
    internal class UnitsAuthoring : MonoBehaviour
    {
        private class UnitsAuthoringBaker : Baker<UnitsAuthoring>
        {
            public override void Bake(UnitsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Unit());
            }
        }
    }

    public struct Unit : IComponentData
    {
    }
}