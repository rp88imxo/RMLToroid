using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;

namespace RML
{
    public class Bullet : SpriteEntity, IBullet
    {
        #region PRIVATE_FIELDS
        private BulletPool originPool;
        private Coroutine bulletDurationCoroutine;
        #endregion


        protected override void HandleTriggerEnter()
        {
            base.HandleTriggerEnter();
            if (bulletDurationCoroutine != null)
            {
                StopCoroutine(bulletDurationCoroutine);
            }

            PublishBulletHitEvent();

            Recycle();
        }

        private void PublishBulletHitEvent()
        {
            EventBusMessager.Instance.PublishEvent(new BulletHitPayload(this));
        }

        #region INTERFACE_CONTRACTS
        public ObjectPool OriginPool
        {
            get => originPool;
            set
            {
                if (originPool == null)
                {
                    originPool = value as BulletPool;
                    Debug.Assert(originPool != null, "Can't downcast pool!");
                }
                else
                {
                    Debug.LogError("Attempt to redefine the origin pool!");
                }
            }
        }

        public GameObject GameObject => gameObject;

        public void Recycle()
        {
            SetBorderPacker(false);
            SetTriggerEnterState(false);
            originPool.Reclaim(this);
        }

        public void ResetState()
        {
            spriteRigidbody.velocity = Vector2.zero;
            SetBorderPacker(true);
            SetTriggerEnterState(true);
        }

        public void ShootBullet(float shootPower, float duration, Vector2 shootDirection)
        {
            spriteRigidbody.AddForce(shootDirection * shootPower);
            bulletDurationCoroutine = StartCoroutine(BulletDuration(duration));
        }
        #endregion

        public void SetPos(Vector2 pos)
        {
            transform.position = pos;
        }

        IEnumerator BulletDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            Recycle();
        }

    }
}

