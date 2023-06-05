using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Listener/PlayerListener")]
public class PlayerListener : ScriptableSingleton<PlayerListener>
{
    private float hitPoints = 20.0f;
    private float currentHitPoints = 20.0f;
    private float defaultAttack = 1.0f;
    private float defaultWeaponAttack = 1.0f;
    private float defaultDefence = 5.0f;
    private float defaultMagicDefence = 0;
    private float speed = 1;
    private float criticalRate = 0.2f;
    private float criticalDamage = 1.0f;
    private float globalTime = 0f;
    private List<int> effectHeld = new List<int>();
    private Dictionary<int, int> monsterList = new Dictionary<int, int>();
    private int[] weapon1 = new int[2];
    private int[] weapon2 = new int[2];

    public float HitPoints => hitPoints;
    public float CurrentHitPoints => currentHitPoints;
    public float DefaultAttack => defaultAttack;
    public float DefaultWeaponAttack => defaultWeaponAttack;
    public float DefaultDefence => defaultDefence;
    public float DefaultMagicDefence => defaultMagicDefence;
    public float Speed => speed;
    public float CriticalRate => criticalRate;
    public float CriticalDamage => criticalDamage;
    public float GlobalTime => globalTime;

    public int[] Weapon1 => weapon1;
    public int[] Weapon2 => weapon2;

    public IReadOnlyList<int> EffectHeld => effectHeld;
    public IReadOnlyDictionary<int, int> MonsterList => monsterList;

    public void Reset()
    {
        effectHeld.Clear();
        monsterList.Clear();
    }

    public void UpdateTime(float _time)
    {
        globalTime = _time;
    }

    public void UpdateWeapons(int weapon1Index, int weapon1Level, int weapon2Index, int weapon2Level)
    {
        weapon1[0] = weapon1Index;
        weapon1[1] = weapon1Level;
        weapon2[0] = weapon2Index;
        weapon2[1] = weapon2Level;
    }

    public void UpdateDict(int key)
    {
        if (monsterList.ContainsKey(key))
        {
            monsterList[key] += 1;
        }
        else
        {
            monsterList[key] = 1;
        }
    }

    public void UpdatePlayerHP(float current, float max)
    {
        currentHitPoints = current;
        hitPoints = max;
    }

    public void UpdatePlayerStats(Players player)
    {
        defaultAttack = player.DefaultAttack;
        defaultWeaponAttack = player.DefaultWeaponAttack;
        defaultDefence = player.DefaultDefence;
        defaultMagicDefence = player.DefaultMagicDefence;
        speed = player.Speed;
        criticalRate = player.CriticalRate;
        criticalDamage = player.CriticalDamage;
        if (criticalRate > 1)
        {
            criticalRate = 1;
        }
    }

    public void UpdatePlayerEffects(int index)
    {
        effectHeld.Add(index);
        effectHeld.Sort();
    }
}
