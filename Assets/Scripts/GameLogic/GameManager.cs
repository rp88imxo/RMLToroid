using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using RML.Messaging;
using RML.Utils;


namespace RML
{
    public class GameManager : MonoBehaviour
    {
        #region EXPOSED_FIELDS
        [SerializeField]
        private UFOPool uFOPool;
        [SerializeField]
        private AsteroidPool asteroidPool;
        [SerializeField]
        private PlayerFactory playerFactory;
        [SerializeField]
        private SoundData soundDataSettings;
        [SerializeField]
        private Vector2 playerSpawnPos = Vector2.zero;
        [SerializeField]
        private int maxLives = 3;

        [SerializeField]
        private TextMeshProUGUI gameOverText;
        [SerializeField]
        private TextMeshProUGUI liveText;
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        GameObject buttonPannel;
        [SerializeField]
        private Button playBtn;
        [SerializeField]
        private Button exitBtn;
        #endregion

        private IUFOSpawner uFOSpawner;
        private IAsteroidSpawner asteroidSpawner;
        private IPlayerShipSpawner playerShipSpawner;

        private ICameraHelper cameraHelper;
        private IShip playerShip;
        private IBorderPacker borderPacker;
        private IGameStateHelper gameStateHelper;
        private ISoundSpawner soundSpawner;

        private UIManager uiManager;

        private void Start()
        {
            InitOtherEntities();
            
            InitDependicies();
            InitSpawners();

            SetupEvents(true);
            StartGame();
        }

        private void InitDependicies()
        {
            cameraHelper = new CameraHelper(Camera.main, 5);
            borderPacker = new BorderPacker(cameraHelper);
            gameStateHelper = new GameStateHelper(maxLives);
        }

        private void InitOtherEntities()
        {
            uiManager = new UIManager(gameOverText, liveText, scoreText, buttonPannel, playBtn, exitBtn);
        }

        private void InitSpawners()
        {
            asteroidSpawner = new AsteroidSpawner(2, asteroidPool, this, 5f, cameraHelper);
            uFOSpawner = new UFOSpawner(uFOPool, this, cameraHelper.GetScreenRect(), 10f);
            playerShipSpawner = new PlayerShipSpawner(playerFactory);
            soundSpawner = new SoundSpawner(soundDataSettings, this);
        }

        private void StartGame()
        {
            playerShipSpawner.Spawn(playerSpawnPos);
            uFOSpawner.Spawn();
            asteroidSpawner.Spawn();
        }

        private void SetupEvents(bool isSubscribe)
        {
            if (isSubscribe)
            {
                EventBusMessager.Instance.AddListener(GameConsts.PlayerShipSpawned, OnPlayerShipSpawned);

                //EventBusMessager.Instance.AddListener(GameConsts.UfoDestroyed, OnUfoDestroyed);
                //EventBusMessager.Instance.AddListener(GameConsts.GameEnd, OnGameEnd);
                //EventBusMessager.Instance.AddListener(GameConsts.Restart, OnGameRestart);
                //EventBusMessager.Instance.AddListener(GameConsts.PlayerShipSpawned, OnPlayerShipSpawned);
            }
            else
            {
                EventBusMessager.Instance.RemoveListener(GameConsts.PlayerShipSpawned, OnPlayerShipSpawned);
                //EventBusMessager.Instance.RemoveListener(GameConsts.UfoDestroyed, OnUfoDestroyed);
                //EventBusMessager.Instance.RemoveListener(GameConsts.GameEnd, OnGameEnd);
                //EventBusMessager.Instance.RemoveListener(GameConsts.Restart, OnGameRestart);
                //EventBusMessager.Instance.RemoveListener(GameConsts.PlayerShipSpawned, OnPlayerShipSpawned);
            }
        }

        private void OnPlayerShipSpawned(IPayload obj)
        {
            var data = (PlayerShipSpawnedPayload)obj;
            playerShip = data.Ship;
            playerShip.SetDependecies(gameStateHelper);
        }

        #region UNITY_METHODS
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                EventBusMessager.Instance.PublishEvent(new RestartPayload(GameConsts.Restart));
            }
            borderPacker.UpdateState();  
        }
        #endregion
    }

}
