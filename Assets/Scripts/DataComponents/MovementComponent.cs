using Unity.Entities;

namespace Shooter.ECS
{
    [GenerateAuthoringComponent]
    struct MovementData : IComponentData
    {
        public int xDirection;
        public int zDirection;
        public float speed;
    }

    public class MovementComponent { }
}
