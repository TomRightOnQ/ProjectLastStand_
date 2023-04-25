using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<T>(typeof(T).Name);
                if (instance == null)
                {
                    instance = ScriptableObject.CreateInstance<T>();
                    Debug.LogWarning("Created instance of " + typeof(T).Name + " because none was found in resources.");
                }
            }
            return instance;
        }
    }
}
