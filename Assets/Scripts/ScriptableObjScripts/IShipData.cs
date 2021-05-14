using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface IShipData
    {
        float GetCurrentTurnPower();
        float GetCurrenMovePower();
        float GetHyperjumpCooldown();
        float GetMaxSpeed();
        float GetSpawnTimeDelay();
    }
}

