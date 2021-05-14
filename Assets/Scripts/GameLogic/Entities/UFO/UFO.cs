using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;

namespace RML
{
    public class UFO : SpriteEntity, IUFO
    {
        #region EXPOSED_FIELDS
        [SerializeField]
        private Transform gunPivot;
        [SerializeField]
        private GunData gunDataSettings;
        [SerializeField]
        private UFOData UFODataSettings;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private BulletPool bulletPool;
        #endregion

        private float spawnYDispersion = 0.7f;
        private Coroutine shootCoroutine;
        private UFOPool originPool;
        private Transform playerShip;
        private Rect borderRect;

        private IGun gun;
        private IUFOData ufoData;
        private IGunData gunData;

        private float TimeBetweenShoots => gunData.GetTimeBetweenShots();     

        #region INTERFACE_CONTRACTS
        public ObjectPool OriginPool 
        {
            get => originPool;
            set
            {
                if (originPool == null)
                {
                    originPool = value as UFOPool;
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

            PublishUFODestroyEvent();
            ShootPlayerTask(false);

            originPool.Reclaim(this);
        }

        public void ResetState()
        {
            spriteRigidbody.velocity = Vector2.zero;
            SetBorderPacker(true);
            SetTriggerEnterState(true);
        }

        public void SetPlayerShip(Transform ship)
        {
            playerShip = ship;
        }

        public void SetupBorders(Rect rect)
        {
            borderRect = rect;
        }

        public void UFOInit()
        {
            int border = Random.Range(0f, 1f) <= 0.5f ? 1 : -1;
            SetTriggerEnterState(true);
            SetBorderPacker(true);

            SetActiveState(true);
            SetPosition(border);
            SetVelocity(border);

            ShootPlayerTask(true);
        }

        #endregion

        protected override void HandleTriggerEnter()
        {
            base.HandleTriggerEnter();
            HandleDestroy();
        }

        private void HandleDestroy()
        {
            Recycle();
        }

        private void PublishUFODestroyEvent()
        {
            EventBusMessager.Instance.PublishEvent(new UFODestroyedPayload(transform.position, this));
        }

        public override void Init()
        {
            base.Init();
            ufoData = UFODataSettings;
            gunData = gunDataSettings;
            gun = new Gun(gunData, this, bulletPool);
        }

        private void SetActiveState(bool state)
        {
            spriteCollider.enabled = state;
            spriteRenderer.enabled = state;
            audioSource.enabled = state;
            spriteRigidbody.velocity = Vector2.zero;
            gameObject.SetActive(state);
        }

        private void SetVelocity(int border)
        {
            spriteRigidbody.AddForce(transform.right * ufoData.GetMovePower() * border);
        }

        private void SetPosition(int border)
        {
            transform.position = new Vector3(border * GetXSpawnPos(), GetYSpawnPos());
        }

        private float GetXSpawnPos()
        {
            var bounds =  spriteRenderer.bounds.extents;
            return borderRect.x + bounds.x;
        }
        private float GetYSpawnPos()
        {
            return borderRect.y * Random.Range(-spawnYDispersion, spawnYDispersion);
        }

        private void ShootPlayerTask(bool isShouldShoot)
        {

            if (isShouldShoot)
            {
                shootCoroutine= StartCoroutine(ShootCor(TimeBetweenShoots));
            }
            else
            {
                if (shootCoroutine != null)
                {
                    StopCoroutine(shootCoroutine);
                }
            }

            gun.SetState(isShouldShoot);
        }

        IEnumerator ShootCor(float timeBetweenShots)
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenShots);
                Shoot();
            }
        }

        private void Shoot()
        {
            var shootDir = GetShootDir();
            gun.Shoot(gunPivot.position, shootDir);
        }

        private Vector2 GetShootDir()
        {
            if (playerShip != null)
            {
                var dir = (playerShip.position - transform.position).normalized;
                var dispersion = Random.Range(ufoData.GetPrecisionAngleMin(), ufoData.GetPrecisionAngleMax());
                dir = Quaternion.Euler(0f, 0f, dispersion) * dir;
                return dir;
            }
            else
            {
                Debug.Assert(true, "Palyer Ship has been null");
                return Vector2.zero;
            }
        }
    }
}

