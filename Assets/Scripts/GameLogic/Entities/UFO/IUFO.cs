using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML
{
    public interface IUFO : IPoolable
    {
        void SetPlayerShip(Transform ship); 
        void SetupBorders(Rect rect);
        void UFOInit();
    }
}

