using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Entities are all NPCs and player-controlled units
public abstract class Entities : DefaultObjects
{

    // Player Stats
    [SerializeField] protected float hitPoints = 20;
    [SerializeField] protected float currentHitPoints = 20;
    [SerializeField] protected float defaultAttack = 1;
    [SerializeField] protected float defaultWeaponAttack = 1;
    [SerializeField] protected float defaultDefence = 5;
    [SerializeField] protected float defaultMagicDefence = 0;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected Slider hpS;

    // Getters and Setters
    public float HitPoints
    {
        get { return hitPoints; }
        set { hitPoints = value; }
    }

    public float CurrentHitPoints
    {
        get { return currentHitPoints; }
        set { currentHitPoints = value; }
    }

    public float DefaultAttack
    {
        get { return defaultAttack; }
        set { defaultAttack = value; }
    }

    public float DefaultWeaponAttack
    {
        get { return defaultWeaponAttack; }
        set { defaultWeaponAttack = value; }
    }

    public float DefaultDefence
    {
        get { return defaultDefence; }
        set { defaultDefence = value; }
    }

    public float DefaultmagicDefence
    {
        get { return defaultMagicDefence; }
        set { defaultMagicDefence = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // Update
    void Update() {

    }

    // Taking Damage
    public void TakeDamage(float damage) {

        currentHitPoints -= damage;
        if (currentHitPoints > hitPoints) {
            currentHitPoints = hitPoints;
        }
        UpdateHP();
    }

    // Dying
    public void Deactivate()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(10f, -10f, -10f);
    }

    // HP Bar 
    public void UpdateHP()
    {
        hpS.maxValue = hitPoints;
        hpS.value = currentHitPoints;
    }
}
