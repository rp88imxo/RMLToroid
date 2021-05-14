using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface IBullet : IPoolable
    {
        void ShootBullet(float shootPower, float duration, Vector2 shootDirection);
    }
}
