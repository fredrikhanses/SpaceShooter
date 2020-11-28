using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace Shooter.ECS
{
    public class EnemyShootSystem : SystemBase
    {
        EntityQuery group;

        protected override void OnCreate()
        {
            // Cached access to a set of ComponentData based on a specific query
            group = GetEntityQuery(typeof(EnemyTag), typeof(ShootDelayData));
        }

        //[BurstCompile]
        struct ShootJob : IJobChunk
        {
            public float deltaTime;
            public ComponentTypeHandle<ShootDelayData> ShootDelayDataTypeHandle;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                NativeArray<ShootDelayData> chunkShootDelayData = chunk.GetNativeArray(ShootDelayDataTypeHandle);
                for (var i = 0; i < chunk.Count; i++)
                {
                    ShootDelayData shootDelayData = chunkShootDelayData[i];
                    float shootDelay = shootDelayData.delay;
                    shootDelay -= deltaTime;
                    if (shootDelay <= 0.0f)
                    {
                        //GameManager.gameManager.EnemyShoot();
                    }
                    chunkShootDelayData[i] = new ShootDelayData
                    {
                        delay = shootDelay
                    };
                }
            }
        }

        // OnUpdate runs on the main thread.
        protected override void OnUpdate()
        {
            ComponentTypeHandle<ShootDelayData> shootDelayDataType = GetComponentTypeHandle<ShootDelayData>();
            ShootJob shootJob = new ShootJob()
            {
                deltaTime = Time.DeltaTime,
                ShootDelayDataTypeHandle = shootDelayDataType
            };
            shootJob.Run(group);
        }
    }
}