using Dots.Authoring;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Dots.System
{
    internal partial struct UnitMoverSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var unitMoverJob = new UnitMoverJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            unitMoverJob.ScheduleParallel();
        }
    }

    public partial struct UnitMoverJob : IJobEntity
    {
        public float DeltaTime;

        public void Execute(ref LocalTransform localTransform, in UnitMover unitMover,
            ref PhysicsVelocity physicsVelocity)
        {
            var moveDirection = unitMover.TargetPosition - localTransform.Position;

            var reachedTargetDistance = 2;
            if (math.lengthsq(moveDirection) < reachedTargetDistance)
            {
                //reached target position
                physicsVelocity.Linear = float3.zero;
                physicsVelocity.Angular = float3.zero;
                return;
            }

            moveDirection = math.normalize(moveDirection);

            localTransform.Rotation =
                math.slerp(
                    localTransform.Rotation,
                    quaternion.LookRotation(moveDirection, math.up()),
                    DeltaTime * unitMover.RotationSpeed);

            physicsVelocity.Linear = moveDirection * unitMover.MoveSpeed;
            physicsVelocity.Angular = float3.zero;
        }
    }
}