using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hold the Configuration of all monsters
[CreateAssetMenu(menuName = "Manager/PrefabManager")]
public class PrefabManager : ScriptableSingleton<MonsterConfigs>
{
    public GameObject monsterPrefab;
    public GameObject projPrefab;
    public GameObject playerPrefab;
    public GameObject weaponPrefab;
    public GameObject damageNumberPrefab;
}
