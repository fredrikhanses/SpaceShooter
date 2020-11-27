using Unity.Entities;

namespace Shooter.ECS
{
    [GenerateAuthoringComponent]  
    public struct MoveSpeed : IComponentData
    {
        public float Value;
    }

    public class MoveSpeedComponent { }
}
