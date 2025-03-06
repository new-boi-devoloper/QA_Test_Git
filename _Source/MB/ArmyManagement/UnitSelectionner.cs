using Dots.Authoring;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MB.ArmyManagement
{
    public interface IUnitSelectionner
    {
        void SelectUnits(Vector3 mouseWorldPosition);
    }

    public class UnitSelectionner : IUnitSelectionner
    {
        public void SelectUnits(Vector3 mouseWorldPosition)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected, UnitMover>()
                .Build(entityManager);

            var entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            var unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
            var movePositionArray = GenerateMovePositionArray(mouseWorldPosition, entityArray.Length);

            for (var i = 0; i < unitMoverArray.Length; i++)
            {
                var unitMover = unitMoverArray[i];
                unitMover.TargetPosition = movePositionArray[i];
                unitMoverArray[i] = unitMover;
            }

            entityQuery.CopyFromComponentDataArray(unitMoverArray);
        }

        private NativeArray<float3> GenerateMovePositionArray(float3 targetPosition, int positionCount)
        {
            var positionArray = new NativeArray<float3>(positionCount, Allocator.Temp);
            if (positionCount == 0) return positionArray;

            positionArray[0] = targetPosition;
            if (positionCount == 1) return positionArray;

            var ringSize = 2.2f;
            var ring = 0;
            var postionIndex = 0;

            while (postionIndex < positionCount)
            {
                var ringPositionCount = 3 + ring * 2;
                for (var i = 0; i < ringPositionCount; i++)
                {
                    var angle = i * (math.PI2 / ringPositionCount);
                    var ringVector = math.rotate(quaternion.RotateY(angle), new float3(ringSize * (ring + 1), 0, 0));
                    var ringPosition = targetPosition + ringVector;

                    positionArray[postionIndex] = ringPosition;
                    postionIndex++;

                    if (postionIndex >= positionCount) break;
                }

                ring++;
            }


            return positionArray;
        }
    }
}