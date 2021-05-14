using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;

namespace RML
{
    public class AsteroidSpawner : IAsteroidSpawner
    {
        
        private AsteroidPool asteroidPool;
        private List<Asteroid> asteroids;
        private int smallAsteroidCount;
        private MonoBehaviour parrentBehaviour;
        private Coroutine spawnCor;
        private float timeBetweenSpawn;
        private Transform playerShipData;

        private ICameraHelper cameraHelper;

        public AsteroidSpawner(
            int smallAsteroidCount, 
            AsteroidPool asteroidPool, 
            MonoBehaviour parrentBehaviour, 
            float timeBetweenSpawn,
            ICameraHelper cameraHelper)
        {
            this.cameraHelper = cameraHelper;
            this.timeBetweenSpawn = timeBetweenSpawn;
            this.parrentBehaviour = parrentBehaviour;
            this.asteroidPool = asteroidPool;
            this.smallAsteroidCount = smallAsteroidCount;
            asteroids = new List<Asteroid>();
            SetupEvents(true);
        }

        ~AsteroidSpawner()
        {
            SetupEvents(false);
        }

        private void SetupEvents(bool isSubscribe)
        {
            if (isSubscribe)
            {
                EventBusMessager.Instance.AddListener(GameConsts.AsteroidDestroyed, OnAsteroidDestroyed);
                EventBusMessager.Instance.AddListener(GameConsts.GameEnd, OnGameEnd);
                EventBusMessager.Instance.AddListener(GameConsts.PlayerShipSpawned, OnPlayerShipSpawned);
                EventBusMessager.Instance.AddListener(GameConsts.Restart, OnGameRestart);
            }
            else
            {
                EventBusMessager.Instance.RemoveListener(GameConsts.AsteroidDestroyed, OnAsteroidDestroyed);
                EventBusMessager.Instance.RemoveListener(GameConsts.GameEnd, OnGameEnd);
                EventBusMessager.Instance.RemoveListener(GameConsts.PlayerShipSpawned, OnPlayerShipSpawned);
                EventBusMessager.Instance.RemoveListener(GameConsts.Restart, OnGameRestart);
            }
        }

        private void OnGameRestart(IPayload obj)
        {
            Spawn();
        }

        private void OnPlayerShipSpawned(IPayload obj)
        {
            var data = (PlayerShipSpawnedPayload)obj;
            playerShipData = data.Ship.GetTransform(); 
        }

        public void Spawn()
        {
            spawnCor = parrentBehaviour.StartCoroutine(AsteroidSpawnCor(timeBetweenSpawn));
        }

        IEnumerator AsteroidSpawnCor(float timeBetweenSpawn)
        {
            while (true)
            {
                SpawnAsteroid(AsteroidType.Big, GetAsteroidSpawnPos());
                yield return new WaitForSeconds(timeBetweenSpawn);
            }
        }

        private Vector2 GetAsteroidSpawnPos()
        {
           return cameraHelper.GetRandomPosExcludeTarget(playerShipData.position, 15f);
        }

        private void SpawnAsteroid(AsteroidType asteroidType, Vector2 spawnPos)
        {
            var asteroid = asteroidPool.Get(asteroidType);
            asteroids.Add(asteroid);
            asteroid.SetPosition(spawnPos);
            asteroid.ThrowAsteroid();
        }

        private void SpawnSmallAsteroids(AsteroidType asteroidType, Vector2 spawnPos)
        {
            var asteroid = asteroidPool.Get(asteroidType);
            asteroids.Add(asteroid);
            asteroid.SetPosition(spawnPos);
            asteroid.ThrowAsteroid();
        }

        private void OnGameEnd(IPayload payload)
        {
            if (spawnCor != null)
            {
                parrentBehaviour.StopCoroutine(spawnCor);
            }
            RecycleAllAsteroids();
        }

        private void OnAsteroidDestroyed(IPayload payload)
        {
            var asteroidDestroyedPayload = (AsteroidDestroyedPayload)payload;
            asteroids.Remove(asteroidDestroyedPayload.Asteroid as Asteroid);
            if (asteroidDestroyedPayload.AsteroidType == AsteroidType.Big)
            {
                for (int i = 0; i < smallAsteroidCount; i++)
                {
                    SpawnSmallAsteroids(AsteroidType.Small, asteroidDestroyedPayload.Pos);
                }
            }
        }

        private void RecycleAllAsteroids()
        {
            asteroids.ForEach(x => x.Recycle());
            asteroids.Clear();
        }

    }

}
