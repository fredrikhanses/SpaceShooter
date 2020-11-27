//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Transforms;
//using UnityEngine;

//namespace Shooter.ECS
//{
//    [AlwaysSynchronizeSystem]
//    public class PlayerShootSystem : JobComponentSystem
//    {
//        protected override JobHandle OnUpdate(JobHandle inputDeps)
//        {
//            Entities.ForEach((in InputData inputData) =>
//            {
//                if (Input.GetKey(inputData.fireKey))
//                {
//                    EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
//                    Entity bulletEntity = entityManager.Instantiate(GameManager.gameManager.playerBulletPrefab);
//                    //entityManager.SetComponentData(bulletEntity, new Translation { Value = new float3(0.0f, 0.0f, 0.0f) });
//                    //entityManager.SetComponentData(bulletEntity, new MoveSpeed { Value = 20.0f });
//                }
//            }).Run();
//            return default;
//        }
//    }
//}
