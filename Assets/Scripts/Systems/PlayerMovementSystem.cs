using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Shooter.ECS
{
    [AlwaysSynchronizeSystem]
    public class PlayerMovementSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {   
            float deltaTime = Time.DeltaTime;
            float topBound = GameManager.gameManager.topBound;
            float bottomBound = GameManager.gameManager.bottomBound;
            float rightBound = GameManager.gameManager.rightBound;
            float leftBound = GameManager.gameManager.leftBound;
            Entities.ForEach((ref Translation translation, in MovementData moveData, in PlayerTag playerTag) =>
            {
                translation.Value.x = math.clamp(translation.Value.x + (moveData.speed * moveData.xDirection * deltaTime), leftBound, rightBound);
                translation.Value.z = math.clamp(translation.Value.z + (moveData.speed * moveData.zDirection * deltaTime), bottomBound, topBound);
                GameManager.gameManager.playerTranslation.x = translation.Value.x;
                GameManager.gameManager.playerTranslation.z = translation.Value.z;
            }).Run();
            return default;
        }
    }
}
