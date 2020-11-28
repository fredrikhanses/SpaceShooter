using Unity.Entities;

namespace Shooter.ECS
{
    [GenerateAuthoringComponent]
    struct ShootDelayData : IComponentData
    {
        public float delay;
    }

    public class EnemyShootDelayComponent { }
}

