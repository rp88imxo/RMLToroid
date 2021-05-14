using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    [CreateAssetMenu(menuName = "RML/GameData/UFOData")]
    public class UFOData : ScriptableObject, IUFOData
    {
        #region EXPOSED_FIELDS
        [SerializeField]
        private float movePower;
        [SerializeField]
        private float minPrecisionAngle;
        [SerializeField]
        private float maxPrecisionAngle;
        #endregion


        #region INTERFACE_CONTRACTS
        public float GetMovePower()
        {
            return movePower;
        }

        public float GetPrecisionAngleMax()
        {
            return maxPrecisionAngle;
        }

        public float GetPrecisionAngleMin()
        {
            return minPrecisionAngle;
        }
        #endregion

    }
}

