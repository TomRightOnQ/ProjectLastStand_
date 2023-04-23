using UnityEngine;

public class SingletonClass<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance = null;

    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            // If there are another instance is running, destroy this new one.
        }
        else
        {
            Instance = gameObject.GetComponent<T>();
        }
    }
}
