using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Interacts with Online Settings
[CreateAssetMenu(menuName = "Manager/MasterManager")]
public class MasterManager : ScriptableSingleton<MasterManager>
{
    [SerializeField] private GameSettings gameSettings;

    public static GameSettings GameSettings
    {
        get { return Instance.gameSettings; }
    }
}
