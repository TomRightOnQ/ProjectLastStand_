using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Loading
public class PersistentConfigManager : MonoBehaviour
{
    private static PersistentConfigManager instance;

    private bool isLoaded = false;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        UpgradeConfigs.Instance.InitEffect();
        yield return new WaitForSeconds(1f);
        isLoaded = true;
        Debug.Log("ready");
        SceneManager.LoadScene("MainMenu");
    }

    // Method to check if the loading process is complete
    public static bool IsLoaded()
    {
        return instance != null && instance.isLoaded;
    }
}
