using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML
{
    public interface IGun
    {
        void Shoot(Vector3 instantinationPos, Vector3 dir);
        void SetState(bool enableToShoot);
    }

}
