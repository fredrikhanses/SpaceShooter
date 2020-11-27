using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

namespace Shooter.ECS
{
    public class GameManager : MonoBehaviour
    {
        #region GAME_MANAGER_VARIABLES
        public static GameManager gameManager;
        [Header("Simulation Settings")]
        public float topBound = 16.5f;
        public float bottomBound = -13.5f;
        public float leftBound = -23.5f;
        public float rightBound = 23.5f;
        [Header("Enemy Settings")]
        public GameObject enemyShipPrefab;
        public float enemySpeed = 1.0f;
        [Header("Spawn Settings")]
        public int enemyShipCount = 1;
        public int enemyShipIncrement = 1;
        //FPS fps;
        private int count;

        private void Awake()
        {
            if (gameManager != null && gameManager != this)
            {
                Destroy(gameObject);
                return;
            }
            gameManager = this;
        }
        #endregion GAME_MANAGER_VARIABLES
        //TransformAccessArray transforms;
        //MovementJob moveJob;
        //JobHandle moveHandle;
        EntityManager entityManager;

        //private void OnDisable()
        //{
        //  moveHandle.Complete();
        //  transforms.Dispose();
        //}

        private void Start()
        {
           
            //fps = GetComponent<FPS>();
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //transforms = new TransformAccessArray(0, -1);
            AddShips(enemyShipCount);
        }

        private void Update()
        {
            //moveHandle.Complete();
            if (Input.GetMouseButtonDown(0))
            {
                AddShips(enemyShipIncrement);
            }
            //moveJob = new MovementJob()
            //{
            //    moveSpeed = enemySpeed,
            //    topBound = topBound,
            //    bottomBound = bottomBound,
            //    deltaTime = Time.deltaTime
            //};
            //moveHandle = moveJob.Schedule(transforms);
            //JobHandle.ScheduleBatchedJobs();
            //Debug.Log(count);
        }

        private void AddShips(int amount)
        {
            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            Entity entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemyShipPrefab, settings);
            NativeArray<Entity> entities;// = new NativeArray<Entity>(amount, Allocator.Temp);
            //entityManager.Instantiate(entityPrefab, entities);
            entities = entityManager.Instantiate(entityPrefab, amount, Allocator.Temp);
            //moveHandle.Complete();
            //transforms.capacity = transforms.length + amount;
            for (int i = 0; i < amount; i++)
            {
                float xValue = UnityEngine.Random.Range(leftBound, rightBound);
                float zValue = UnityEngine.Random.Range(0.0f, 10.0f);
                entityManager.SetComponentData(entities[i], new Translation { Value = new float3(xValue, 0.0f, topBound + zValue) });
                entityManager.SetComponentData(entities[i], new Rotation { Value = new quaternion(0.0f, 1.0f, 0.0f, 0.0f) });
                entityManager.SetComponentData(entities[i], new MoveSpeed { Value = enemySpeed });
                //Vector3 position = new Vector3(xValue, 0.0f, zValue + topBound);
                //Quaternion rotation = Quaternion.Euler(0.0f, 180f, 0.0f);
                //GameObject enemyShip = Instantiate(enemyShipPrefab, position, rotation);
                //transforms.Add(enemyShip.transform);
            }
            entities.Dispose();
            count += amount;
            //fps.SetElementCount(count);
        }
    }
}
