using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface IBulletSpawner
    {
        void Spawn(Vector2 spawnPos, float shootPower, float shootDuration, Vector2 shootDirection);
    }
}
