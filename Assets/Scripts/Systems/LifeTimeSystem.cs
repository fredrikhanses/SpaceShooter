using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Shooter.ECS
{
    //public class LifeTimeSystem : JobComponentSystem
    //{
    //    protected override JobHandle OnUpdate(JobHandle inputDeps)
    //    {
    //        float deltaTime = Time.DeltaTime;
    //        Entities.ForEach((ref LifeTimeData lifeTimeData) =>
    //        {
    //            float lifeTimeRemaining =  lifeTimeData.lifeTimeRemaining -= deltaTime;
    //            if (lifeTimeRemaining <= 0.0f)
    //            {

    //            }
    //        }).Schedule(inputDeps);
    //        return inputDeps;
    //    }
    //}

    class LifetimeSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        protected override void OnCreate()
        {
            base.OnCreate();
            // Find the ECB system once and store it for later usage
            m_EndSimulationEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            // Acquire an ECB and convert it to a concurrent one to be able
            // to use it from a parallel job.
            float deltaTime = Time.DeltaTime;
            var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().AsParallelWriter();
            Entities.ForEach((Entity entity, int entityInQueryIndex, ref LifeTimeData lifetime) =>
            {
                // Track the lifetime of an entity and destroy it once
                // the lifetime reaches zero
                if (lifetime.lifeTimeRemaining <= 0)
                {
                    // pass the entityInQueryIndex to the operation so
                    // the ECB can play back the commands in the right
                    // order
                    ecb.DestroyEntity(entityInQueryIndex, entity);
                }
                else
                {
                    lifetime.lifeTimeRemaining -= deltaTime;
                }
            }).ScheduleParallel();

            // Make sure that the ECB system knows about our job
            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}