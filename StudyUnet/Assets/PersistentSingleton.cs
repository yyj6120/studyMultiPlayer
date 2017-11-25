using UnityEngine;
using System.Collections;

public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}

