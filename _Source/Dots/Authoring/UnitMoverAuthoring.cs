using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Dots.Authoring
{
    public class UnitMoverAuthoring : MonoBehaviour
    {
        public int moveSpeedAuth;
        public int rotationSpeedAuth;
        public float3 TargetPositionAuth;


        public class Baker : Baker<UnitMoverAuthoring>
        {
            public override void Bake(UnitMoverAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                var gameObjectTransform = GetComponent<Transform>();
                float3 startPosition = gameObjectTransform.position;

                AddComponent(entity, new UnitMover
                {
                    MoveSpeed = authoring.moveSpeedAuth,
                    RotationSpeed = authoring.rotationSpeedAuth,
                    TargetPosition = startPosition
                });
            }
        }
    }

    public struct UnitMover : IComponentData
    {
        public int MoveSpeed;
        public int RotationSpeed;
        public float3 TargetPosition;
    }
}