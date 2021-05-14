using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ObjectPool : ScriptableObject
{
    protected Scene scene;

    protected T CreateGameObjectInstance<T>(T prefab, bool moveToObjectPoolScene) where T : MonoBehaviour
    {
        if (!scene.isLoaded)
        {
            if (Application.isEditor)
            {
                scene = SceneManager.GetSceneByName(name);
                if (!scene.isLoaded)
                {
                    scene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                scene = SceneManager.CreateScene(name);
            }
        }

        T objectInstance = Instantiate(prefab);
        if (moveToObjectPoolScene)
        {
            SceneManager.MoveGameObjectToScene(objectInstance.gameObject, scene);
        }
        return objectInstance;
    }
}
