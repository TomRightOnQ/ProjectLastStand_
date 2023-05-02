using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Online game settings
[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string gameVersion = "Dev 0.1";
    [SerializeField] private string nickName = "Player";

    public string GameVersion {
        get { return gameVersion; }
        set { gameVersion = value; }
    }

    public string NickName
    {
        get { return nickName + Random.Range(0, 9999); }
        set { nickName = value.ToString(); }
    }
}
