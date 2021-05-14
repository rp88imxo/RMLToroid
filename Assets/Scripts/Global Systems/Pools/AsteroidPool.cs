using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using RML.Utils;

namespace RML
{
    [CreateAssetMenu(menuName = "RML/Pools/AsteroidPool")]
    public class AsteroidPool : ObjectPool
    {

        [SerializeField]
        Asteroid[] asteroidPrefabsBig;
        
        [SerializeField]
        Asteroid[] asteroidPrefabsSmall;

        [SerializeField]
        bool recycle;

        [System.NonSerialized]
        List<Asteroid>[] pools;

        int TotalBigPrefabs => asteroidPrefabsBig.Length;
        int TotalSmallPrefabs => asteroidPrefabsBig.Length;

        int TotalTypes => GameConsts.TotalAsteroidTypes;

        void CreatePools()
        {
            pools = new List<Asteroid>[TotalTypes];
            for (int i = 0; i < TotalTypes; i++)
            {
                pools[i] = new List<Asteroid>();
            }

        }

        public Asteroid Get(AsteroidType asteroidType)
        {
            Asteroid asteroidObject;
            int index = (int)asteroidType;

            if (recycle)
            {
                if (pools == null || !SceneManager.GetSceneByName(name).isLoaded)
                {
                    CreatePools();
                }

                int lastIndex = pools[index].Count - 1;
                if (lastIndex < 0)
                {
                    asteroidObject = GetAsteroidByType(asteroidType, true);
                    asteroidObject.Init();
                    asteroidObject.OriginPool = this;
                }
                else
                {
                    asteroidObject = pools[index][lastIndex];
                    asteroidObject.GameObject.SetActive(true);
                    asteroidObject.ResetState();
                    pools[index].RemoveAt(lastIndex);
                }
            }
            else
            {
                asteroidObject = GetAsteroidByType(asteroidType, false);
                asteroidObject.Init();
                asteroidObject.OriginPool = this;
            }

            return asteroidObject;
        }

        private Asteroid GetAsteroidByType(AsteroidType asteroidType, bool moveToObjectPoolScene)
        {
            Asteroid asteroidObject;
            switch (asteroidType)
            {
                case AsteroidType.Big:
                    asteroidObject = CreateGameObjectInstance(asteroidPrefabsBig[Random.Range(0, TotalBigPrefabs)], moveToObjectPoolScene);
                    break;
                case AsteroidType.Small:
                    asteroidObject = CreateGameObjectInstance(asteroidPrefabsSmall[Random.Range(0, TotalSmallPrefabs)], moveToObjectPoolScene);
                    break;
                default:
                    asteroidObject = CreateGameObjectInstance(asteroidPrefabsBig[Random.Range(0, TotalBigPrefabs)], moveToObjectPoolScene);
                    break;
            }

            return asteroidObject;
        }

        public void Reclaim(Asteroid asteroidObject)
        {
            if (asteroidObject.OriginPool != this)
            {
                Debug.LogError("Operating an object from different Origin Pool!");
            }

            if (recycle)
            {
                if (pools == null || !SceneManager.GetSceneByName(name).isLoaded)
                {
                    CreatePools();
                }

                pools[(int)asteroidObject.AsteroidType].Add(asteroidObject);
                asteroidObject.GameObject.SetActive(false);

            }
            else
            {
                Destroy(asteroidObject.GameObject);
            }

        }

    }
}
