using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance = null;
    public static T Instance
    {
        get { return _instance; }
    }

    protected static void SetInstance(T inst)
    {
        if (_instance == null)
        {
            _instance = inst;
        }
    }
}
