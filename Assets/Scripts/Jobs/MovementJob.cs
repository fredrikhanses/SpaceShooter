using Unity.Burst;
using UnityEngine;
using UnityEngine.Jobs;

namespace Shooter.ECS
{
    [BurstCompile]
    public struct MovementJob : IJobParallelForTransform
    {
        public float moveSpeed;
        public float topBound;
        public float bottomBound;
        public float deltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 position = transform.position;
            position += moveSpeed * deltaTime * (transform.rotation * new Vector3(0.0f, 0.0f, 1.0f));
            if (position.z < bottomBound)
            {
                position.z = topBound;
            }
            transform.position = position;
        }
    }
}
