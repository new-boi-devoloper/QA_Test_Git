using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dots.Authoring
{
    public class SelectedAuthoring : MonoBehaviour
    {
        public GameObject visualGameObject;
        [FormerlySerializedAs("showScale")] public float scale;


        private class SelectedUnitBaker : Baker<SelectedAuthoring>
        {
            public override void Bake(SelectedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Selected
                {
                    VisualEntity = GetEntity(authoring.visualGameObject, TransformUsageFlags.Dynamic),
                    showScale = authoring.scale
                });
                SetComponentEnabled<Selected>(entity, false);
            }
        }
    }

    public struct Selected : IComponentData, IEnableableComponent
    {
        public Entity VisualEntity;
        public float showScale;
    }
}