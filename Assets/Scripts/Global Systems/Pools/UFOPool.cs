using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RML
{
    [CreateAssetMenu(menuName = "RML/Pools/UFO Pool")]
    public class UFOPool : ObjectPool
    {
        [SerializeField]
        UFO[] ufoPrefabs;

        [SerializeField]
        private bool recycle;

        [System.NonSerialized]
        List<UFO> pools;

        private int Total => ufoPrefabs.Length;

        void CreatePools()
        {
            pools = new List<UFO>();
        }

        public UFO Get(int index)
        {
            if (index >= Total || index < 0)
            {
                return null;
            }

            UFO ufoObject;

            if (recycle)
            {
                if (pools == null || !SceneManager.GetSceneByName(name).isLoaded)
                {
                    CreatePools();
                }

                int lastIndex = pools.Count - 1;
                if (lastIndex < 0)
                {
                    ufoObject = CreateGameObjectInstance(ufoPrefabs[index], true);
                    ufoObject.Init();
                    ufoObject.OriginPool = this;
                }
                else
                {
                    ufoObject = pools[lastIndex];
                    ufoObject.GameObject.SetActive(true);
                    ufoObject.ResetState();
                    pools.RemoveAt(lastIndex);
                }
            }
            else
            {
                ufoObject = CreateGameObjectInstance(ufoPrefabs[index], false);
                ufoObject.Init();
                ufoObject.OriginPool = this;
            }

            return ufoObject;
        }

        public UFO GetRandom()
        {
            return Get(Random.Range(0, Total));
        }

        public void Reclaim(UFO ufo)
        {
            if (ufo.OriginPool != this)
            {
                Debug.LogError("Operating an object from different Origin Pool!");
            }

            if (recycle)
            {
                if (pools == null || !SceneManager.GetSceneByName(name).isLoaded)
                {
                    CreatePools();
                }

                pools.Add(ufo);
                ufo.GameObject.SetActive(false);
            }
            else
            {
                Destroy(ufo.GameObject);
            }

        }

    }
}

