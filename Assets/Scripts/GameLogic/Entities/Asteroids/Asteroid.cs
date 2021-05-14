using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;

namespace RML
{
    public class Asteroid : SpriteEntity, IAsteroid
    {
        #region EXPOSED_FIELDS
        [SerializeField]
        private AsteroidType asteroidType;
        [SerializeField]
        private AsteroidData asteroidDataSettings;
        #endregion

        #region PRIVATE_FIELDS
        private AsteroidPool originPool;
        private IAsteroidData asteroidData;
        #endregion

        #region PUBLIC_PROPERTIES
        public AsteroidType AsteroidType => asteroidType;
        #endregion

        #region INTERFACE_CONTRACTS
        public GameObject GameObject => gameObject;
        public ObjectPool OriginPool 
        { 
            get => originPool;
            set 
            {
                if (originPool == null)
                {
                    originPool = value as AsteroidPool;
                    Debug.Assert(originPool != null, "Can't downcast pool!");
                }
                else
                {
                    Debug.LogError("Attempt to redefine the origin pool!");
                }
            }
        }

        public void Recycle()
        {
            SetBorderPacker(false);
            SetTriggerEnterState(false);
            originPool.Reclaim(this);
        }

        public void ThrowAsteroid()
        {
            var force =
                Random.insideUnitCircle * Random.Range(asteroidData.GetMinPower(), asteroidData.GetMaxPower());
            spriteRigidbody.AddForce(force);
            var turn = Random.Range(asteroidData.GetMinTurn(), asteroidData.GetMaxTurn());
            spriteRigidbody.SetRotation(turn);
        }
        public void ResetState()
        {
            SetBorderPacker(true);
            SetTriggerEnterState(true);
            spriteRigidbody.velocity = Vector2.zero;
            
        }
        #endregion

        public override void Init()
        {
            base.Init();
            asteroidData = asteroidDataSettings;
        }

        public void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        protected override void HandleTriggerEnter()
        {
            base.HandleTriggerEnter();
            PublishAsteroidDestroyed();
            Recycle();
        }

        private void PublishAsteroidDestroyed()
        {
            EventBusMessager.Instance.PublishEvent(new AsteroidDestroyedPayload(asteroidType, transform.position, this));
        }
    }
}

