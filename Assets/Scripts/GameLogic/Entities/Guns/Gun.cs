using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;


namespace RML
{
    public class Gun : IGun
    {
        private BulletPool bulletPool;
        private float 
            duration,
            timeBetweenShots,
            speed;

        private bool onCooldown;
        private MonoBehaviour parrentMonoBehaviour;

        public Gun(IGunData gunData, MonoBehaviour parrentMonoBehaviour, BulletPool bulletPool)
        {
            duration = gunData.GetDuration();
            timeBetweenShots = gunData.GetTimeBetweenShots();
            speed = gunData.GetSpeed();
            this.bulletPool = bulletPool;

            this.parrentMonoBehaviour = parrentMonoBehaviour;
            onCooldown = false;
        }

        IEnumerator HandleCooldown()
        {
            onCooldown = true;
            yield return new WaitForSeconds(timeBetweenShots);
            onCooldown = false;
        }

        #region INTERFACE_CONTRACTS
        public void SetState(bool enableToShoot)
        {
            onCooldown = !enableToShoot; // TODO: Test condition
        }

        public void Shoot(Vector3 instantinationPos, Vector3 dir)
        {
            if (!onCooldown)
            {
                var bullet = bulletPool.GetRandom();
                bullet.Init();
                bullet.SetPos(instantinationPos);
                bullet.ShootBullet(speed, duration, dir);
                parrentMonoBehaviour.StartCoroutine(HandleCooldown());
                PublishShootEvent();
            }
        }
        #endregion

        private void PublishShootEvent()
        {
            EventBusMessager.Instance.PublishEvent(new EmptyPayload(GameConsts.ShootBullet));
        }
    }
}
