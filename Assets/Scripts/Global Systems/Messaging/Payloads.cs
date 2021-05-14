using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Utils;

namespace RML.Messaging
{
    public readonly struct EmptyPayload : IPayload
    {
        public string EventName { get; }
        public EmptyPayload(string eventName)
        {
            this.EventName = eventName;
        }

    }
    public readonly struct BorderPackerPayload : IPayload
    {
        public bool State { get; }

        public IBorderPackable BorderPackable { get; }
        public string EventName { get; }


        public BorderPackerPayload(IBorderPackable borderPackable, bool state)
        {
            EventName = GameConsts.SetBorderPacker;
            BorderPackable = borderPackable;
            State = state;
        }
    }
    public readonly struct ShipDestroyedPayload : IPayload
    {
        public string EventName { get; }
        public Vector2 Pos { get; }

        public ShipDestroyedPayload(Vector2 pos)
        {
            EventName = GameConsts.ShipDestroyed;
            Pos = pos;
        }
    }

    public readonly struct AsteroidDestroyedPayload : IPayload
    {
        public string EventName { get; }
        public AsteroidType AsteroidType { get; }
        public Vector2 Pos { get; }
        public IAsteroid Asteroid { get; }
        public AsteroidDestroyedPayload(AsteroidType asteroidType, Vector2 pos, IAsteroid asteroid)
        {
            EventName = GameConsts.AsteroidDestroyed;
            AsteroidType = asteroidType;
            Pos = pos;
            Asteroid = asteroid;
        }
    }

    public readonly struct BulletHitPayload : IPayload
    {
        public string EventName { get; }
        public IBullet Bullet { get; }

        public BulletHitPayload(IBullet bullet)
        {
            Bullet = bullet;
            EventName = GameConsts.BulletHit;
        }
       
    }

    public readonly struct UFODestroyedPayload : IPayload
    {
        public string EventName { get; }
        public Vector2 Pos { get; }
        public IUFO UFO { get; }

        public UFODestroyedPayload(Vector2 pos, IUFO UFO)
        {
            this.UFO = UFO;
            Pos = pos;
            EventName = GameConsts.UfoDestroyed;
        }
    }

    public readonly struct PlayerShipSpawnedPayload : IPayload
    {
        public string EventName { get; }
        public IShip Ship { get; }
        public PlayerShipSpawnedPayload(IShip ship)
        {
            EventName = GameConsts.PlayerShipSpawned;
            Ship = ship;
        }

    }

    public readonly struct LivesChangedPayload : IPayload
    {
        public string EventName { get; }
        public int NewLivesCount { get; }

        public LivesChangedPayload(int newLivesCount)
        {
            NewLivesCount = newLivesCount;
            EventName = GameConsts.LivesChanged;
        }
    }

    public readonly struct ScoreChangedPayload : IPayload
    {
        public string EventName { get; }
        public int NewScore { get; }
        public ScoreChangedPayload(int newScore)
        {
            NewScore = newScore;
            EventName = GameConsts.ScoreChanged;
        }
    }

    public readonly struct RestartPayload : IPayload
    {
        public string EventName { get; }
        public RestartPayload(string eventName)
        {
            EventName = eventName;
        }
    }

    public readonly struct ApplicationExitPayload : IPayload
    {
        public string EventName { get; }
        public ApplicationExitPayload(string eventName)
        {
            EventName = eventName;
        }
    }
}
