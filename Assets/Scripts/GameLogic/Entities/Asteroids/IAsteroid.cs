using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface IAsteroid : IPoolable
    {
        void ThrowAsteroid();
    }
}

