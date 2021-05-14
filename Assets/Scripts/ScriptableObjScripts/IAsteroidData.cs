using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface IAsteroidData
    {
        float GetMaxPower();
        float GetMinPower();
        float GetMaxTurn();
        float GetMinTurn();
    }
}

