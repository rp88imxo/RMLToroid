using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML
{
    [CreateAssetMenu(fileName = "New Asteroid Data", menuName = "RML/GameData/AsteroidData")]
    public class AsteroidData : ScriptableObject, IAsteroidData
    {
        #region EXPOSED_FIELDS
        [SerializeField]
        private float maxPower;
        [SerializeField]
        private float minPower;
        [SerializeField]
        private float maxTurn;
        [SerializeField]
        private float minTurn;
        #endregion


        #region INTERFACE_CONTRACTS
        public float GetMaxPower()
        {
            return maxPower;
        }

        public float GetMaxTurn()
        {
            return maxTurn;
        }

        public float GetMinPower()
        {
            return minPower;
        }

        public float GetMinTurn()
        {
            return minTurn;
        }
        #endregion
    }
}
