using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface IBorderPackable
    {
        Transform GetSpriteTransform();
        Bounds GetSpriteBounds();
    }
}

