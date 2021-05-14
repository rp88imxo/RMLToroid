using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using RML.Messaging;
using RML.Utils;
using System;

namespace RML
{
    public class UIManager
    {
        private TextMeshProUGUI gameOverText;
        private TextMeshProUGUI liveText;
        private TextMeshProUGUI scoreText;

        private GameObject buttonPannel;
        private Button playBtn;
        private Button exitBtn;

        public UIManager
            (
                TextMeshProUGUI gameOverText,
                TextMeshProUGUI liveText,
                TextMeshProUGUI scoreText,
                GameObject buttonPannel,
                Button playBtn,
                Button exitBtn
            )
        {
            this.gameOverText = gameOverText;
            this.liveText = liveText;
            this.scoreText = scoreText;
            this.buttonPannel = buttonPannel;
            this.playBtn = playBtn;
            this.exitBtn = exitBtn;

            SetupEvents(true);
        }

        ~UIManager()
        {
            SetupEvents(false);
        }

        private void SetupEvents(bool isSubscribe)
        {
            if (isSubscribe)
            {
                playBtn.onClick.AddListener(OnPlayAgainBtnClicked);
                exitBtn.onClick.AddListener(OnExitBtnClicked);

                EventBusMessager.Instance.AddListener(GameConsts.LivesChanged, OnLivesChanged);
                EventBusMessager.Instance.AddListener(GameConsts.ScoreChanged, OnScoreChanged);
                EventBusMessager.Instance.AddListener(GameConsts.GameEnd, OnGameEnd);
            }
            else
            {
                playBtn.onClick.RemoveListener(OnPlayAgainBtnClicked);
                exitBtn.onClick.RemoveListener(OnExitBtnClicked);

                EventBusMessager.Instance.RemoveListener(GameConsts.LivesChanged, OnLivesChanged);
                EventBusMessager.Instance.RemoveListener(GameConsts.ScoreChanged, OnScoreChanged);
                EventBusMessager.Instance.RemoveListener(GameConsts.GameEnd, OnGameEnd);
            }
        }

        private void OnGameEnd(IPayload obj)
        {
            SetOnGameEndMenuState(true);
        }

        private void OnScoreChanged(IPayload obj)
        {
            var data = (ScoreChangedPayload)obj;
            scoreText.text = data.NewScore.ToString();
        }

        private void OnLivesChanged(IPayload obj)
        {
            var data = (LivesChangedPayload)obj;
            liveText.text = data.NewLivesCount.ToString();
        }

        private void OnExitBtnClicked()
        {
            EventBusMessager.Instance.PublishEvent(new ApplicationExitPayload(GameConsts.Exit));
        }

        private void OnPlayAgainBtnClicked()
        {
            SetOnGameEndMenuState(false);
            EventBusMessager.Instance.PublishEvent(new RestartPayload(GameConsts.Restart));
        }

        private void SetOnGameEndMenuState(bool state)
        {
            buttonPannel.SetActive(state);
            gameOverText.gameObject.SetActive(state);
        }
    }

}
