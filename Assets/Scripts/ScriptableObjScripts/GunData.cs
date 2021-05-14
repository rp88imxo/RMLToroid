using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML
{
    [CreateAssetMenu(fileName = "New GunData", menuName = "RML/GameData/Gun Data")]
    public class GunData : ScriptableObject, IGunData
    {
        [SerializeField]
        private float duration;
        [SerializeField]
        private float timeBetweenShots;
        [SerializeField]
        private float speed;

        public float GetDuration()
        {
            return duration;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public float GetTimeBetweenShots()
        {
            return timeBetweenShots;
        }
    }
}
