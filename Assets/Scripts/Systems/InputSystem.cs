using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Shooter.ECS
{
    [AlwaysSynchronizeSystem]
    public class InputSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Entities.ForEach((ref MovementData moveData, in InputData inputData) =>
            {
                moveData.xDirection = 0;
                moveData.zDirection = 0;
                moveData.xDirection += Input.GetKey(inputData.rightKey) ? 1 : 0;
                moveData.xDirection -= Input.GetKey(inputData.leftKey) ? 1 : 0;
                moveData.zDirection += Input.GetKey(inputData.upKey) ? 1 : 0;
                moveData.zDirection -= Input.GetKey(inputData.downKey) ? 1 : 0;
            }).Run();
            return default;
        }
    }
}
