using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML
{
    public class CameraHelper : ICameraHelper
    {
        private Camera camera;
        private Rect screenRect;
        private int maxAttemtps;

        public CameraHelper(Camera camera, int maxAttemptsToCheckSafety)
        {
            this.camera = camera;
            this.maxAttemtps = maxAttemptsToCheckSafety;

            Vector2 worldMin = GetWorldPointFromViewport(Vector2.zero);
            Vector2 worldMax = GetWorldPointFromViewport(Vector2.one);
            screenRect =  Rect.MinMaxRect(worldMin.x, worldMin.y, worldMax.x, worldMax.y);
        }
        
        #region INTERFACE_CONTRACTS
        public Rect GetScreenRect()
        {
            return screenRect;
        }

        public Vector2 GetWorldPointFromViewport(in Vector2 viewportPos)
        {
            return camera.ViewportToWorldPoint(viewportPos);
        }
        public Vector2 GetRandomPosWithinScreenRect()
        {
            return camera.ViewportToWorldPoint(RandomOneVector());
        }

        public Vector2 GetRandomPosExcludeTarget(in Vector2 target, float distanceFromTarget)
        {
            int attempts = 0;
            Vector2 randomPos = GetRandomPosWithinScreenRect();

            while (!CheckSafetyFromTarget(randomPos, target, distanceFromTarget) || attempts < maxAttemtps)
            {
                attempts++;
                randomPos = GetRandomPosWithinScreenRect();
            }

            return randomPos;
            
        }
        #endregion

        private bool CheckSafetyFromTarget(Vector2 pos, Vector2 target, float distance)
        {
            return Vector2.Distance(pos, target) > distance;
        }

        private Vector2 RandomOneVector()
        {
            return new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

    }
}

