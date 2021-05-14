using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML
{
    public interface IPoolable
    {
        ObjectPool OriginPool
        {
            get;
            set;
        }

        GameObject GameObject
        {
            get;
        }

        void Recycle();
        void ResetState();

    }
}

