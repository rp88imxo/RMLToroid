using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;

namespace RML
{
    public class Ship : SpriteEntity, IShip
{
        #region EXPOSED_PARAMETERS
        [SerializeField]
        private ShipData shipDataSettings;
        [SerializeField]
        private GunData gunDataSettings;
        [SerializeField]
        private Transform gunPivot;
        [SerializeField]
        private BulletPool bulletPool;
        #endregion


        private IShipInputable shipInput;
        private IShipData shipData;
        private IGun gun;
        private IGameStateHelper gameStateHelper;

        #region PRIVATE_FIELDS
        private Vector2 currentMovePower;
        private float currentTurn;
        #endregion

        #region PRIVATE_PROPERTIES
        private bool HasLives => gameStateHelper.GetAvailableLives() > 0;
        #endregion

        #region UNITY_API
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
        }

        private void FixedUpdate()
        {
            HandleMove();
        }

        #endregion


        private void SetupEvents(bool isSubscribe)
        {
            if (isSubscribe)
            {
                EventBusMessager.Instance.AddListener(GameConsts.Restart, OnGameRestart);
            }
            else
            {
                EventBusMessager.Instance.RemoveListener(GameConsts.Restart, OnGameRestart);
            }
        }

        private void OnGameRestart(IPayload obj)
        {
            SetSpawnPos(Vector2.zero);
            SpawnShip();
        }

        #region INTERFACE_CONTRACTS

        public void SetDependecies(IGameStateHelper gameStateHelper)
        {
            this.gameStateHelper = gameStateHelper;
        }

        public override void Init()
        {
            base.Init();
            shipInput = new ShipInput();
            shipData = shipDataSettings;
            gun = new Gun(gunDataSettings, this, bulletPool);

            SetupEvents(true);
        }
        public Transform GetTransform()
        {
            return transform;
        }

        public void SetSpawnPos(Vector2 spawnPos)
        {
            transform.position = spawnPos;
        }
        #endregion


        protected override void HandleTriggerEnter()
        {
            base.HandleTriggerEnter();
            PublishShipDestroyed();
            SetActiveState(false);
            if (HasLives)
            {
                StartCoroutine(SpawnCor());
            }
        }

        IEnumerator SpawnCor()
        {
            yield return new WaitForSeconds(shipData.GetSpawnTimeDelay());

            SpawnShip();
        }

        private void SpawnShip()
        {
            SetSpawnPos(Vector2.zero);
            spriteRigidbody.velocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            SetBorderPacker(true);
            SetTriggerEnterState(true);
            SetActiveState(true);
        }

        private void SetActiveState(bool state)
        {
            shipInput.SetControllPossibility(state);
            spriteCollider.enabled = state;
            spriteRenderer.enabled = state;
        }

        private void PublishShipDestroyed()
        {
            EventBusMessager.Instance.PublishEvent(new ShipDestroyedPayload(transform.position));
        }

        void HandleInput()
        {
            if (CanControlShip())
            {
                if (shipInput.isPressedShootButton())
                {
                    HandleShoot();
                }
                if (shipInput.isPressedHyperJumpButton())
                {
                    HandleHyperjump();
                }

                currentMovePower = transform.up * shipInput.GetVerticalInput() * shipData.GetCurrenMovePower();
                currentTurn = shipInput.GetHorizontalInput() * shipData.GetCurrentTurnPower();
            }
        }

        private void HandleMove()
        {
            if (CanControlShip())
            {
                spriteRigidbody.AddForce(currentMovePower);
                spriteRigidbody.velocity =
                    Vector2.ClampMagnitude(spriteRigidbody.velocity, shipData.GetMaxSpeed());
                spriteRigidbody.SetRotation(CurrentRotation + currentTurn);
            }
        }

        private bool CanControlShip()
        {
            return shipInput != null && shipInput.CanControl();
        }

        private void HandleHyperjump()
        {
            // TODO: добавить гиперпрыжок
        }

        private void HandleShoot()
        {
            gun.Shoot(gunPivot.position, transform.up);
        }
    }
}

