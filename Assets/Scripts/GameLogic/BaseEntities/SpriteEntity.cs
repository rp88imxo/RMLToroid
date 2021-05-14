using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;

namespace RML
{
    [RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
    public class SpriteEntity : MonoBehaviour, IBorderPackable
    {
        protected SpriteRenderer spriteRenderer;
        protected Rigidbody2D spriteRigidbody;
        protected Collider2D spriteCollider;

        protected float CurrentRotation => spriteRigidbody.rotation;

        private bool triggetState;

        #region UNITY_API
       
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (triggetState == true)
            {
                HandleTriggerEnter();
            }
        }

        #endregion

        public virtual void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRigidbody = GetComponent<Rigidbody2D>();
            spriteCollider = GetComponent<Collider2D>();
            

            SetTriggerEnterState(true);
            SetBorderPacker(true);
        }

        protected virtual void HandleTriggerEnter()
        {
            SetBorderPacker(false);
            SetTriggerEnterState(false);
        }

        protected void SetBorderPacker(bool state)
        {
            PublishBorderPackerEvent(state);
        }

        protected void SetTriggerEnterState(bool triggerState)
        {
            triggetState = triggerState;
        }

        private void PublishBorderPackerEvent(bool state)
        {
            var payload = new BorderPackerPayload(this, state);
            EventBusMessager.Instance.PublishEvent(payload);
        }

        #region INTERFACE_CONTRACTS
        public Transform GetSpriteTransform()
        {
            return transform;
        }

        public Bounds GetSpriteBounds()
        {
            return spriteRenderer.bounds;
        }
        #endregion

    }
}

