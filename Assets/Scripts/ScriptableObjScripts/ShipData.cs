using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    [CreateAssetMenu(fileName = "New Ship Data", menuName = "RML/GameData/Ship Data")]
    public class ShipData : ScriptableObject, IShipData
    {
        [SerializeField] 
        private float maxSpeed;
        [SerializeField] 
        private float movePower;
        [SerializeField] 
        private float turnPower;
        [SerializeField] 
        private float hyperjumpCooldown;
        [SerializeField]
        private float SpawnDelaySeconds;


        public float GetMaxSpeed() => maxSpeed;
        public float GetCurrenMovePower() => movePower;
        public float GetCurrentTurnPower() => turnPower;
        public float GetHyperjumpCooldown() => hyperjumpCooldown;
        public float GetSpawnTimeDelay() => SpawnDelaySeconds;
    }

}
