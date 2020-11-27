using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace Shooter.ECS
{
    public class EnemyMovementSystem : SystemBase
    {
        EntityQuery group;

        protected override void OnCreate()
        {
            // Cached access to a set of ComponentData based on a specific query
            group = GetEntityQuery(typeof(EnemyTag), typeof(Translation), typeof(Rotation)/*ComponentType.ReadOnly<Rotation>()*/, ComponentType.ReadOnly<MoveSpeed>());
        }

        //[BurstCompile]
        struct MovementJob : IJobChunk
        {
            public float topBound;
            public float bottomBound;
            public float deltaTime;
            public ComponentTypeHandle<Translation> TranslationTypeHandle;
            /*[ReadOnly]*/ public ComponentTypeHandle<Rotation> RotationTypeHandle;
            [ReadOnly] public ComponentTypeHandle<MoveSpeed> MoveSpeedTypeHandle;

            //public void Execute(ref Translation position, [ReadOnly] ref Rotation rotation, [ReadOnly] ref MoveSpeed moveSpeed)
            //{
            //    float3 value = position.Value;
            //    value += deltaTime * moveSpeed.Value * math.forward(rotation.Value);
            //    if (value.z < bottomBound)
            //    {
            //        value.z = topBound;
            //    }
            //    position.Value = value;
            //}

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                NativeArray<Translation> chunkTranslations = chunk.GetNativeArray(TranslationTypeHandle);
                NativeArray<Rotation> chunkRotations = chunk.GetNativeArray(RotationTypeHandle);
                NativeArray<MoveSpeed> chunkMoveSpeeds = chunk.GetNativeArray(MoveSpeedTypeHandle);
                for (var i = 0; i < chunk.Count; i++)
                {
                    Translation translation = chunkTranslations[i];
                    Rotation rotation = chunkRotations[i];
                    MoveSpeed moveSpeed = chunkMoveSpeeds[i];
                    float3 value = translation.Value;
                    value += deltaTime * moveSpeed.Value * new float3(0.0f, 0.0f, -1.0f);//math.forward(rotation.Value);
                    if (value.z < bottomBound)
                    {
                        value.z = topBound;
                    }
                    chunkTranslations[i] = new Translation
                    {
                        Value = value
                    };
                    chunkRotations[i] = new Rotation
                    {
                        Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(math.up(), moveSpeed.Value * deltaTime))
                    };
                }
            }
        }
        //protected override JobHandle OnUpdate(JobHandle inputDeps)
        //{
        //    MovementJob moveJob = new MovementJob
        //    {
        //        topBound = GameManager.gameManager.topBound,
        //        bottomBound = GameManager.gameManager.bottomBound,
        //        deltaTime = Time.DeltaTime
        //    };
        //    JobHandle moveHandle = moveJob.Schedule(this, inputDeps);
        //    return moveHandle;
        //}


        // OnUpdate runs on the main thread.
        protected override void OnUpdate()
        {
            ComponentTypeHandle<Translation> translationType = GetComponentTypeHandle<Translation>();
            ComponentTypeHandle<Rotation> rotationType = GetComponentTypeHandle<Rotation>();
            ComponentTypeHandle<MoveSpeed> moveSpeedType = GetComponentTypeHandle<MoveSpeed>(true);

            MovementJob moveJob = new MovementJob()
            {
                TranslationTypeHandle = translationType,
                RotationTypeHandle = rotationType,
                MoveSpeedTypeHandle = moveSpeedType,
                deltaTime = Time.DeltaTime,
                topBound = GameManager.gameManager.topBound,
                bottomBound = GameManager.gameManager.bottomBound,
            };

            Dependency = moveJob.Schedule(group, Dependency);
        }
    }
}