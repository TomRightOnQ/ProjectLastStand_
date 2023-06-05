using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

// Pause and show...
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private TextMeshProUGUI timerText;
    private IReadOnlyList<int> effectList;
    private float globalTime = 0f;

    private void OnEnable()
    {
        effectList = PlayerListener.Instance.EffectHeld;
        showEffects();
        UpdateTime();
    }

    private void OnDisable()
    {
        ClearEffects();
    }

    private void showEffects()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            GameObject effectObj = Instantiate(PrefabManager.Instance.BuffIcon);
            effectObj.transform.SetParent(content.transform);
            effectObj.transform.localScale = Vector3.one;
            effectObj.GetComponent<BuffIcons>().SetUp(effectList[i]);
        }
    }

    private void ClearEffects()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Transform child = content.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void UpdateTime()
    {
        globalTime = PlayerListener.Instance.GlobalTime;
        int hours = Mathf.FloorToInt(globalTime / 3600);
        int minutes = Mathf.FloorToInt((globalTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(globalTime % 60);

        string formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";
        timerText.text = "Time: " + formattedTime;
    }
}
