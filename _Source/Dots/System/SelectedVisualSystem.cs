using Dots.Authoring;
using Unity.Entities;
using Unity.Transforms;

namespace Dots.System
{
    public partial struct SelectedVisualSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var selected in SystemAPI.Query<RefRO<Selected>>().WithDisabled<Selected>())
            {
                var visualLocalTransform =
                    SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.VisualEntity);
                visualLocalTransform.ValueRW.Scale = 0f;
            }

            foreach (var selected in SystemAPI.Query<RefRO<Selected>>())
            {
                var visualLocalTransform =
                    SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.VisualEntity);
                visualLocalTransform.ValueRW.Scale = selected.ValueRO.showScale;
            }
        }
    }
}