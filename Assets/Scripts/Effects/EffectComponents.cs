using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Effect Components
public class EffectComponents : MonoBehaviour
{
    // Data for specific effect
    protected float life = -1;
    protected float currentLife;
    protected Players player;

    private void Awake()
    {
        player = GetComponent<Players>();
    }

    protected virtual void Update()
    {
        if (life > 0)
        {
            currentLife -= Time.deltaTime;
            if (currentLife <= 0)
            {
                RemoveEffect();
            }
        }
    }

    protected virtual void RemoveEffect()
    {
        Destroy(this);
    }
}

public class Rampage : EffectComponents
{
    private float currentHP;
    private float prevHP;

    private void Awake()
    {
        player = GetComponent<Players>();
        currentHP = player.CurrentHitPoints;
        prevHP = currentHP;
    }

    protected override void Update()
    {
        base.Update();
        if (currentHP < prevHP) {
            Invoke();
            currentHP = prevHP;
        }
        player.UpdateStats();
    }

    public void Invoke() 
    {
        player.AddEffect(-5, 1);
    }
}

public class RampageEffect : EffectComponents
{
    private void Awake()
    {
        player = GetComponent<Players>();
        life = 3f;
        currentLife = life;
        player.DefaultAttack += 0.05f;
        player.DefaultWeaponAttack += 0.05f;
        player.Speed += 0.05f;
    }

    protected override void Update()
    {
        base.Update();
        player.UpdateStats();
    }

    protected override void RemoveEffect()
    {
        player.DefaultAttack -= 0.05f;
        player.DefaultWeaponAttack -= 0.05f;
        player.Speed -= 0.05f;
        Destroy(this);
    }
}

public class RelentlessResent : EffectComponents
{
    private const float DAMAGE = 0.07f;
    private const float SPEED = 0.07f;
    private const float CRITICAL = 0.07f;

    private void Awake()
    {
        player = GetComponent<Players>();
    }

    protected override void Update()
    {
        float hpRatio = player.CurrentHitPoints / player.HitPoints;

        if (hpRatio < 0.8f && player.DamageMod < DAMAGE)
        {
            player.DamageMod = DAMAGE;
        }
        else if (hpRatio >= 0.8f && player.DamageMod >= DAMAGE)
        {
            player.DamageMod = 1f;
        }

        if (hpRatio < 0.5f && player.SpeedMod < SPEED)
        {
            player.SpeedMod = SPEED;
        }
        else if (hpRatio >= 0.5f && player.SpeedMod >= SPEED)
        {
            player.SpeedMod = 1f;
        }

        if (hpRatio < 0.3f && player.CriticalMod < CRITICAL)
        {
            player.CriticalMod = CRITICAL;
        }
        else if (hpRatio >= 0.3f && player.CriticalMod >= CRITICAL)
        {
            player.CriticalMod = 1f;
        }
        player.UpdateStats();
    }
}

public class Immortal : EffectComponents
{
    private float healTimer = 0f;
    private float healRate = 1f;

    private void Awake()
    {
        player = GetComponent<Players>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.CurrentHitPoints < player.HitPoints)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= (healRate))
            {
                player.CurrentHitPoints += 0.1f;
                healTimer = 0f;
            }
        }
        player.UpdateStats();
    }
}

public class MeleeGrandMaster : EffectComponents
{


    private int prevCount = 0;
    private void Awake()
    {
        player = GetComponent<Players>();

        if (player.MeleeCounts == 1)
        {
            player.SpeedMod += 0.2f;
            player.DamageBase += 0.15f;
            player.WeaponDamageBase += 0.15f;
            prevCount = 1;
        }
        else if (player.MeleeCounts == 2)
        {
            player.SpeedMod += 0.4f;
            player.DamageBase += 0.3f;
            player.WeaponDamageBase += 0.3f;
            prevCount = 2;
        }
    }

    protected override void Update()
    {
        int change = player.MeleeCounts - prevCount;
        switch (change)
        {
            case -1:
                player.SpeedMod -= 0.1f;
                player.DamageBase -= 0.15f;
                player.WeaponDamageBase -= 0.15f;
                break;
            case -2:
                player.SpeedMod -= 0.4f;
                player.DamageBase -= 0.3f;
                player.WeaponDamageBase -= 0.3f;
                break;
            case 1:
                player.SpeedMod += 0.2f;
                player.DamageBase += 0.15f;
                player.WeaponDamageBase += 0.15f;
                break;
            case 2:
                player.SpeedMod += 0.4f;
                player.DamageBase += 0.3f;
                player.WeaponDamageBase += 0.3f;
                break;
            default:
                break;
        }
        prevCount = player.MeleeCounts;
        player.UpdateStats();
    }
}

public class Assassination : EffectComponents
{
    private const float CRITCAL_RATE = 0.20f;
    private const float CRITCAL_DAMAGE = 3.5f;
    private const float EXTREME_RATE = 0.01f;
    private const float EXTREME_DAMAGE = 50f;

    private void Awake()
    {
        player = GetComponent<Players>();
    }
    protected override void Update() 
    {
        if (UnityEngine.Random.value < CRITCAL_RATE)
        {
            player.CriticalMod = CRITCAL_DAMAGE;
        }
        else {
            player.CriticalMod = 1;
        }
        if (UnityEngine.Random.value < EXTREME_RATE)
        {
            player.CriticalMod = EXTREME_DAMAGE;
        }
        else
        {
            player.CriticalMod = 1;
        }
        player.UpdateStats();
    }
}
