using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using RML.Utils;

namespace RML
{
    [CreateAssetMenu(menuName = "RML/Pools/Bullet Pool")]
    public class BulletPool : ObjectPool
    {
        [SerializeField]
        Bullet[] bulletPrefabs;

        [SerializeField]
        private bool recycle;

        [System.NonSerialized]
        List<Bullet> pools;

        private int Total => bulletPrefabs.Length;
        
        void CreatePools()
        {
            pools = new List<Bullet>();
        }

        public Bullet Get(int index)
        {
            if (index >= Total || index < 0)
            {
                return null;
            }

            Bullet bulletObject;

            if (recycle)
            {
                if (pools == null || !SceneManager.GetSceneByName(name).isLoaded)
                {
                    CreatePools();
                }

                int lastIndex = pools.Count - 1;
                if (lastIndex < 0)
                {
                    bulletObject = CreateGameObjectInstance(bulletPrefabs[index], true);
                    bulletObject.Init();
                    bulletObject.OriginPool = this;
                }
                else
                {
                    bulletObject = pools[lastIndex];
                    bulletObject.GameObject.SetActive(true);
                    bulletObject.ResetState();
                    pools.RemoveAt(lastIndex);
                }
            }
            else
            {
                bulletObject = CreateGameObjectInstance(bulletPrefabs[index], false);
                bulletObject.Init();
                bulletObject.OriginPool = this;
            }

            return bulletObject;
        }

        public Bullet GetRandom()
        {
            return Get(Random.Range(0, Total));
        }

        public void Reclaim(Bullet bullet)
        {
            if (bullet.OriginPool != this)
            {
                Debug.LogError("Operating an object from different Origin Pool!");
            }

            if (recycle)
            {
                if (pools == null || !SceneManager.GetSceneByName(name).isLoaded)
                {
                    CreatePools();
                }

                pools.Add(bullet);
                bullet.GameObject.SetActive(false);
            }
            else
            {
                Destroy(bullet.GameObject);
            }

        }
    }

}
