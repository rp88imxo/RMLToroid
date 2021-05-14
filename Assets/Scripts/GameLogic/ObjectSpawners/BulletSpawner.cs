using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;

namespace RML
{
    public class BulletSpawner : IBulletSpawner
    {
        private BulletPool bulletPool;
        private List<Bullet> bullets;

        public BulletSpawner(BulletPool bulletPool)
        {
            this.bulletPool = bulletPool;
            bullets = new List<Bullet>();
            SetupEvents(true);
        }

        private void SetupEvents(bool isSubscribe)
        {
            if (isSubscribe)
            {
                EventBusMessager.Instance.AddListener(GameConsts.BulletHit, OnBulletHit);
                EventBusMessager.Instance.AddListener(GameConsts.GameEnd, OnGameEnd);
            }
            else
            {
                EventBusMessager.Instance.RemoveListener(GameConsts.BulletHit, OnBulletHit);
                EventBusMessager.Instance.RemoveListener(GameConsts.GameEnd, OnGameEnd);
            }
        }

        private void OnGameEnd(IPayload payload)
        {
            RecycleAllBullets();
        }

        private void RecycleAllBullets()
        {
            bullets.ForEach(x => x.Recycle());
            bullets.Clear();
        }

        ~BulletSpawner()
        {
            SetupEvents(false);
        }

        private void OnBulletHit(IPayload payload)
        {
            var hitPayload = (BulletHitPayload)payload;
            bullets.Remove(hitPayload.Bullet as Bullet);
        }

        #region INTERFACE_CONTRACTS
        public void Spawn(Vector2 spawnPos, float shootPower, float shootDuration, Vector2 shootDirection)
        {
            var bullet = bulletPool.GetRandom();
            bullet.Init();
            bullet.SetPos(spawnPos);
            bullet.ShootBullet(shootPower, shootDuration, shootDirection);
        }
        #endregion

    }
}

