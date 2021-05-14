using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    [CreateAssetMenu(menuName ="RML/Factory/Player Ship")]
    public class PlayerFactory : ObjectPool
    {
        [SerializeField]
        private Ship playerShipPrefab;

        public Ship GetPlayerShip()
        {
            Ship ship = CreateGameObjectInstance(playerShipPrefab, false);
            return ship;
        }
    }
}

