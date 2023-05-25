using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Effect Components
public class EffectComponents : MonoBehaviour
{
    // Data for specific effect
    protected float life;
    protected float currentLife;
    protected Entities entity;

    private void Awake()
    {
        entity = GetComponent<Entities>();
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

public class Damage : EffectComponents
{
    protected override void Update()
    {
        base.Update();
    }
}

public class EffectPlaceHolder_1 : EffectComponents
{
    private void Awake()
    {
        life = 3f;
        currentLife = life;
    }

    protected override void Update()
    {
        base.Update();
    }
}
