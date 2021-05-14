using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;
using System;

namespace RML
{
    public class BorderPacker : IBorderPacker
    {
        private List<IBorderPackable> borderPackables;
        private ICameraHelper cameraHelper;

        public BorderPacker(ICameraHelper cameraHelper)
        {
            borderPackables = new List<IBorderPackable>();
            this.cameraHelper = cameraHelper;
            
            EventBusMessager.Instance.AddListener(GameConsts.SetBorderPacker, OnBorderPack);
        }

        ~BorderPacker()
        {
            EventBusMessager.Instance.RemoveListener(GameConsts.SetBorderPacker, OnBorderPack);
        }

        #region INTERFACE_CONTRACTS
        public void UpdateState()
        {
            borderPackables.ForEach(x =>
            {
                var bounds = x.GetSpriteBounds();
                var transform = x.GetSpriteTransform();
                PackOnScreen(bounds, ref transform);
            });
        }
        #endregion

        private void OnBorderPack(IPayload payload)
        {
            var borderPackerPayload = (BorderPackerPayload)payload;
            if (borderPackerPayload.State)
            {
                borderPackables.Add(borderPackerPayload.BorderPackable);
            }
            else
            {
                borderPackables.Remove(borderPackerPayload.BorderPackable);
            }
        }

        private void PackOnScreen(in Bounds bounds, ref Transform transform)
        {
            float borderExtentX = bounds.extents.x;
            float borderExtentY = bounds.extents.y;

            Vector2 bottomLeftWorldPos = cameraHelper.GetWorldPointFromViewport(Vector2.zero);
            Vector2 topRightWorldPos = cameraHelper.GetWorldPointFromViewport(Vector2.one);

            Vector2 spritePos = transform.position;

            // Left
            if (spritePos.x + borderExtentX < bottomLeftWorldPos.x)
            {
                spritePos.x = topRightWorldPos.x + borderExtentX;
            }
            // Right
            else if (spritePos.x - borderExtentX > topRightWorldPos.x)
            {
                spritePos.x = bottomLeftWorldPos.x - borderExtentX;
            }
            // Top
            if (spritePos.y - borderExtentY > topRightWorldPos.y)
            {
                spritePos.y = bottomLeftWorldPos.y - borderExtentY;
            }
            // Bottom
            else if (spritePos.y + borderExtentY < bottomLeftWorldPos.y)
            {
                spritePos.y = topRightWorldPos.y + borderExtentY;
            }

            transform.position = spritePos;
        }

    }
}

