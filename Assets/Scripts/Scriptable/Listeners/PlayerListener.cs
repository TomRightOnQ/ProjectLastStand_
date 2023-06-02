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

    public IReadOnlyList<int> EffectHeld => effectHeld;

    public void UpdateTime(float _time)
    {
        globalTime = _time;
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
    }
}
