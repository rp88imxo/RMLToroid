using RML.Messaging;
using RML.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public class SoundSpawner : ISoundSpawner
    {
        private GameObject soundGO;
        private MonoBehaviour parrentBehaviour;
        private List<AudioSource> activeSources;
        private List<AudioSource> pooledSources;
        private int poolDefaultSize = 10;

        private ISoundData soundData;

        public SoundSpawner(SoundData soundData, MonoBehaviour parrentBehave)
        {
            activeSources = new List<AudioSource>();
            pooledSources = new List<AudioSource>();
            soundGO = new GameObject("SoundKeeper");

            this.soundData = soundData;
            this.parrentBehaviour = parrentBehave;
            
            SetupEvents(true);
            RefreshPools(poolDefaultSize);
        }

        ~SoundSpawner()
        {
            SetupEvents(false);
        }

        private void RefreshPools(int poolDefaultSize)
        {
            for (int size = 0; size < poolDefaultSize; size++)
            {
                var source = soundGO.AddComponent<AudioSource>();
                ResetStateOfAudioSource(source);
                pooledSources.Add(source);
            }
        }

        private void ResetStateOfAudioSource(AudioSource source)
        {
            source.enabled = false;
            source.playOnAwake = false;
            source.clip = null;
            source.loop = false;
        }

        private void SetupEvents(bool isSubscribe)
        {
            if (isSubscribe)
            {
                EventBusMessager.Instance.AddListener(GameConsts.AsteroidDestroyed, OnAsteroidDestroyed);
                EventBusMessager.Instance.AddListener(GameConsts.ShipDestroyed, OnShipDestroyed);
                EventBusMessager.Instance.AddListener(GameConsts.UfoDestroyed, OnUfoDestroyed);
                EventBusMessager.Instance.AddListener(GameConsts.ShootBullet, OnShootBullet);
                EventBusMessager.Instance.AddListener(GameConsts.GameEnd, OnGameEnd);
            }
            else
            {
                EventBusMessager.Instance.RemoveListener(GameConsts.AsteroidDestroyed, OnAsteroidDestroyed);
                EventBusMessager.Instance.RemoveListener(GameConsts.ShipDestroyed, OnShipDestroyed);
                EventBusMessager.Instance.RemoveListener(GameConsts.UfoDestroyed, OnUfoDestroyed);
                EventBusMessager.Instance.RemoveListener(GameConsts.ShootBullet, OnShootBullet);
                EventBusMessager.Instance.RemoveListener(GameConsts.GameEnd, OnGameEnd);
            }
        }

        private void OnGameEnd(IPayload obj)
        {
            for (int source = 0; source < activeSources.Count; source++)
            {
                ResetStateOfAudioSource(activeSources[source]);
                RecycleToPool(activeSources[source]);
            }
        }

        private void OnShootBullet(IPayload obj)
        {
            Spawn(SoundType.Shoot);
        }

        private void OnUfoDestroyed(IPayload obj)
        {
            Spawn(SoundType.Explosion);
        }

        private void OnShipDestroyed(IPayload obj)
        {
            Spawn(SoundType.Explosion);
        }

        private void OnAsteroidDestroyed(IPayload obj)
        {
            Spawn(SoundType.Explosion);
        }


        #region INTERFACE_CONTRACTS
        public void Spawn(SoundType soundType)
        {
            var sound = GetSource();
            sound.clip = soundData.GetClip(soundType);
            sound.Play();
            parrentBehaviour.StartCoroutine(PoolReturnCor(sound.clip.length, sound));
        }

        #endregion

        private AudioSource GetSource()
        {
            if (pooledSources.Count == 0)
            {
                RefreshPools(poolDefaultSize);
            }

            var source = pooledSources[0];
            pooledSources.Remove(source);
            activeSources.Add(source);
            source.enabled = true;
            return source;
        }

        IEnumerator PoolReturnCor(float duration, AudioSource source)
        {
            yield return new WaitForSeconds(duration);
            RecycleToPool(source);
        }

        private void RecycleToPool(AudioSource source)
        {
            if (activeSources.Contains(source))
            {
                activeSources.Remove(source);
            }
            if (!pooledSources.Contains(source))
            {
                pooledSources.Add(source);
            }
        }
    }
}

