using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;
using System;
using System.Linq;

namespace RML
{
    public class UFOSpawner : IUFOSpawner
    {
        private UFOPool ufoPool;
        private List<UFO> ufos;
        private MonoBehaviour parrentBehaviour;
        private Coroutine ufoCoroutine;
        private Rect borderRect;
        private Transform playerShipTransform;
        private float timeBetweenSpawns;

        public UFOSpawner(UFOPool ufoPool, MonoBehaviour parrentBehaviour, Rect borderRect, float timeBetweenSpawns)
        {
            this.timeBetweenSpawns = timeBetweenSpawns;
            this.ufoPool = ufoPool;
            this.parrentBehaviour = parrentBehaviour;
            this.borderRect = borderRect;
            ufos = new List<UFO>();

            SetupEvents(true);
        }

        ~UFOSpawner()
        {
            SetupEvents(false);
        }

        private void SetupEvents(bool isSubscribe)
        {
            if (isSubscribe)
            {
                EventBusMessager.Instance.AddListener(GameConsts.UfoDestroyed, OnUfoDestroyed);
                EventBusMessager.Instance.AddListener(GameConsts.GameEnd, OnGameEnd);
                EventBusMessager.Instance.AddListener(GameConsts.Restart, OnGameRestart);
                EventBusMessager.Instance.AddListener(GameConsts.PlayerShipSpawned, OnPlayerShipSpawned);
            }
            else
            {
                EventBusMessager.Instance.RemoveListener(GameConsts.UfoDestroyed, OnUfoDestroyed);
                EventBusMessager.Instance.RemoveListener(GameConsts.GameEnd, OnGameEnd);
                EventBusMessager.Instance.RemoveListener(GameConsts.Restart, OnGameRestart);
                EventBusMessager.Instance.RemoveListener(GameConsts.PlayerShipSpawned, OnPlayerShipSpawned);
            }
        }

        private void OnPlayerShipSpawned(IPayload obj)
        {
            var playerShipSpawnedPayload = (PlayerShipSpawnedPayload)obj;
            playerShipTransform = playerShipSpawnedPayload.Ship.GetTransform();
        }

        private void OnGameRestart(IPayload obj)
        {
            Spawn();
        }

        private void OnGameEnd(IPayload obj)
        {
            if (ufoCoroutine != null)
            {
                parrentBehaviour.StopCoroutine(ufoCoroutine);
            }

            foreach (var item in ufos.ToList())
            {
                item.Recycle();
            }
            ufos.Clear();
        }

        private void OnUfoDestroyed(IPayload obj)
        {
            UFODestroyedPayload uFODestroyedPayload = (UFODestroyedPayload)obj;
            var ufo = uFODestroyedPayload.UFO as UFO;
            ufos.Remove(ufo);
        }

        #region INTERFACE_CONTRACTS
        public void Spawn()
        {
            ufoCoroutine = parrentBehaviour.StartCoroutine(SpawnUfoCor());
        }
        #endregion

        IEnumerator SpawnUfoCor()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenSpawns);
                SpawnUFO();
            }
        }
        
        private void SpawnUFO()
        {
            var ufo = ufoPool.GetRandom();
            ufo.SetPlayerShip(playerShipTransform);
            ufo.SetupBorders(borderRect);
            ufo.UFOInit();
          
            ufos.Add(ufo);
        }
    }

}
