using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RML.Messaging;
using RML.Utils;

namespace RML
{
    public class GameStateHelper : IGameStateHelper
    {
        private int currentScore;
        private int currentAvailableLives;
        private int maxLives;

        public GameStateHelper(int maxLives)
        {
            this.maxLives = maxLives;
            InitState(maxLives);
            SetupEvents(true);
        }

        ~GameStateHelper()
        {
            SetupEvents(false);
        }

        private void InitState(int maxLives)
        {
            SetNewScore(0);
            SetNewLiveCount(maxLives);
        }

        private void SetNewScore(int score)
        {
            currentScore = score;
            PublishScoreChangedEvent(currentScore);
        }

        private void PublishScoreChangedEvent(int currentScore)
        {
            EventBusMessager.Instance.PublishEvent(new ScoreChangedPayload(currentScore));
        }

        private void SetNewLiveCount(int liveCount)
        {
            currentAvailableLives = liveCount;
            PublishLiveCountChangedEvent(currentAvailableLives);
        }

        private void PublishLiveCountChangedEvent(int currentAvailableLives)
        {
            EventBusMessager.Instance.PublishEvent(new LivesChangedPayload(currentAvailableLives));
        }

        private void SetupEvents(bool isSubscribe)
        {
            if (isSubscribe)
            {
                EventBusMessager.Instance.AddListener(GameConsts.Restart, OnGameRestart);
                EventBusMessager.Instance.AddListener(GameConsts.UfoDestroyed, OnUfoDestroyed);
                EventBusMessager.Instance.AddListener(GameConsts.ShipDestroyed, OnPlayerShipDestroyed);
                EventBusMessager.Instance.AddListener(GameConsts.AsteroidDestroyed, OnAsteroidDestroyed);
            }
            else
            {
                EventBusMessager.Instance.RemoveListener(GameConsts.Restart, OnGameRestart);
                EventBusMessager.Instance.RemoveListener(GameConsts.UfoDestroyed, OnUfoDestroyed);
                EventBusMessager.Instance.RemoveListener(GameConsts.ShipDestroyed, OnPlayerShipDestroyed);
                EventBusMessager.Instance.RemoveListener(GameConsts.AsteroidDestroyed, OnAsteroidDestroyed);
            }
        }

        private void OnUfoDestroyed(IPayload obj)
        {
            var data = (UFODestroyedPayload)obj;
            
            AddScore(350);
        }

        private void OnPlayerShipDestroyed(IPayload obj)
        {
            AddLives(-1);
        }

        private void AddLives(int newLives)
        {
            SetNewLiveCount(currentAvailableLives + newLives);
            if (currentAvailableLives == 0)
            {
                PublishGameOverEvent();
            }
        }

        private void PublishGameOverEvent()
        {
            EventBusMessager.Instance.PublishEvent(new EmptyPayload(GameConsts.GameEnd));
        }

        private void OnGameRestart(IPayload obj)
        {
            InitState(maxLives);
        }

        private void OnAsteroidDestroyed(IPayload obj)
        {
            AddScore(100);
        }

        private void AddScore(int score)
        {
            SetNewScore(currentScore + score);
        }


        #region INTERFACE_CONTRACTS
        public int GetAvailableLives()
        {
            return currentAvailableLives;
        }

        public int GetCurrentScore()
        {
            return currentScore;
        }
        #endregion

    }
}

