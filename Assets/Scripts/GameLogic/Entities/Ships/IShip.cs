using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface IShip
    {
        void Init();
        void SetSpawnPos(Vector2 spawnPos);
        Transform GetTransform();
        void SetDependecies(IGameStateHelper gameStateHelper);
    }
}

