using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;

namespace RML
{
    public class PlayerShipSpawner : IPlayerShipSpawner
    {
        private IShip playerShip;

        private PlayerFactory playerFactory;
        private int count;

        public PlayerShipSpawner(PlayerFactory playerFactory)
        {
            this.playerFactory = playerFactory;
            count = 0;
        }

        public void Spawn(Vector2 spawnPos)
        {
            playerShip = playerFactory.GetPlayerShip();
            playerShip.Init();
            playerShip.SetSpawnPos(spawnPos);
            PublishPlayerShipSpawnedEvent();
            count++;
            Debug.Assert(count <= 1, "Warning! Multiply player spawn!");
        }

        private void PublishPlayerShipSpawnedEvent()
        {
            EventBusMessager.Instance.PublishEvent(new PlayerShipSpawnedPayload(playerShip));
        }
    }
}

