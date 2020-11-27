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
        public float topBound = 16.0f;
        public float bottomBound = -16.0f;
        public float leftBound = -34.0f;
        public float rightBound = 34.0f;
        [Header("Player Settings")]
        public GameObject playerShipPrefab;
        public GameObject playerBulletPrefab;
        [Header("Enemy Settings")]
        public GameObject enemyShipPrefab;
        public GameObject enemyBulletPrefab;
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
        public EntityManager entityManager;
        BlobAssetStore blobAssetStore;
        GameObjectConversionSettings settings;
        public float3 playerTranslation;

        private void OnDisable()
        {
        //  moveHandle.Complete();
        //  transforms.Dispose();
            blobAssetStore.Dispose();
        }

        private void Start()
        {
            blobAssetStore = new BlobAssetStore();
            World world = World.DefaultGameObjectInjectionWorld;
            settings = GameObjectConversionSettings.FromWorld(world, blobAssetStore);
            //fps = GetComponent<FPS>();
            entityManager = world.EntityManager;
            //transforms = new TransformAccessArray(0, -1);
            SpawnPlayer();
            AddShips(enemyShipCount);
        }

        private void Update()
        {
            //moveHandle.Complete();
            if (Input.GetMouseButtonDown(0))
            {
                FireWeapon();
            }
            if (Input.GetMouseButtonDown(1))
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

        private void SpawnPlayer()
        {
            Entity playerShipEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerShipPrefab, settings);
            Entity playerShipEntity = entityManager.Instantiate(playerShipEntityPrefab);
            entityManager.SetComponentData(playerShipEntity, new Translation { Value = new float3(0.0f, 0.0f, 0.0f) });
        }

        private void FireWeapon()
        {
            Entity playerBulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerBulletPrefab, settings);
            Entity playerBullet = entityManager.Instantiate(playerBulletEntityPrefab);
            entityManager.SetComponentData(playerBullet, new Translation { Value = playerTranslation + new float3(0.0f, 0.0f, 1.0f) });
        }

        private void AddShips(int amount)
        {
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
