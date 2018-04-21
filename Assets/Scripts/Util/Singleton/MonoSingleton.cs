using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour
{
    public static T Instance;
    public static bool DestroyOnLoad = true;

    protected void OnDestroy()
    {
        if (DestroyOnLoad)
            Instance = default(T);
    }

    protected void DefineSingleton(T parent, bool dontDestroyOnLoad = false)
    {
        if (Instance != null)
        {
            if (dontDestroyOnLoad)
            {
                Destroy(gameObject);
                return;
            }
            
            throw new Exception(string.Format("{0} has already been instantiated. Please make sure you only have one in the scene!", parent.ToString()));
        }


        Instance = parent;

        if (dontDestroyOnLoad)
        {
            DestroyOnLoad = false;
            DontDestroyOnLoad(gameObject);
        }
    }
}