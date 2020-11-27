using Unity.Entities;

namespace Shooter.ECS
{
    [GenerateAuthoringComponent]
    struct LifeTimeData : IComponentData
    {
        public float lifeTimeRemaining;
    }

    public class LifeTimeComponent { }
}

