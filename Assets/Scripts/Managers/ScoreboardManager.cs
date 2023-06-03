using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Control the behavior of the Scoreboard
public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager Instance;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject content;
    private IReadOnlyList<int> effectList;
    private IReadOnlyDictionary<int, int> monsterList;
    private float globalTime = 0f;
    private float xCurrent = -26f;
    private float zCurrent = 1f;
    private int[] weapon1 = new int[2] { 0, 1 };
    private int[] weapon2 = new int[2] { 0, 1 };

    private Vector3 WEAPON_1_POS = new Vector3(7.7f, 1f, 4.3f);
    private Vector3 WEAPON_2_POS = new Vector3(7.7f, 1f, -3.7f);

    public GameObject Content => content;

    public void Awake()
    {
        Instance = this;
        effectList = PlayerListener.Instance.EffectHeld;
        monsterList = PlayerListener.Instance.MonsterList;
        showEffects();
        UpdateTime();
        showWeapons();
        StartCoroutine(showMonsters());
    }

    private void showEffects()
    {
        for (int i = 0; i < effectList.Count; i++) 
        {
            GameObject effectObj = Instantiate(PrefabManager.Instance.BuffIcon);
            effectObj.GetComponent<BuffIcons>().SetUp(effectList[i]);
        }
    }

    private IEnumerator showMonsters()
    {
        foreach (KeyValuePair<int, int> entry in monsterList)
        {
            GameObject resultObj = Instantiate(PrefabManager.Instance.MonsterResult);
            resultObj.transform.position = new Vector3(xCurrent, 0f, zCurrent);
            resultObj.GetComponent<MonsterResult>().SetUp(entry.Key, entry.Value);
            zCurrent -= 3f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void showWeapons()
    {
        GameObject resultObj = Instantiate(PrefabManager.Instance.MonsterResult);
        resultObj.transform.position = WEAPON_1_POS;
        resultObj.GetComponent<MonsterResult>().SetUpWeapon(weapon1[0], weapon1[1]);

        GameObject resultObj2 = Instantiate(PrefabManager.Instance.MonsterResult);
        resultObj2.transform.position = WEAPON_2_POS;
        resultObj2.GetComponent<MonsterResult>().SetUpWeapon(weapon2[0], weapon2[1]);
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

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
