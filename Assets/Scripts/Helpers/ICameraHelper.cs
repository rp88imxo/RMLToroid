using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML
{
    public interface ICameraHelper
    {
        Vector2 GetWorldPointFromViewport(in Vector2 viewportPos);
        Rect GetScreenRect();
        Vector2 GetRandomPosWithinScreenRect();
        Vector2 GetRandomPosExcludeTarget(in Vector2 target, float distanceFromTarget);
    }

}
