using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bullets
public class Projectiles : Items
{
    void Start()
    {
        gameObject.tag = "Proj";
    }

    // Projectile stats
    [SerializeField] private float damage = 1;
    [SerializeField] private int owner = 0;
    [SerializeField] private float life = 1.0f;
    [SerializeField] private bool selfDet = false;
    [SerializeField] private bool player = false;
    [SerializeField] private bool pen = false;
    [SerializeField] private bool aoe = false;
    [SerializeField] private float damageRange = 0.1f;
    private float creationTime;

    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public int Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    public float Life
    {
        get { return life; }
        set { life = value; }
    }

    public bool SelfDet
    {
        get { return selfDet; }
        set { selfDet = value; }
    }

    public bool Player
    {
        get { return player; }
        set { player = value; }
    }

    public bool Pen
    {
        get { return pen; }
        set { pen = value; }
    }

    public bool AOE
    {
        get { return aoe; }
        set { aoe = value; }
    }

    public float DamageRange
    {
        get { return damageRange; }
        set { damageRange = value; }
    }

    // Control the lifespan of a projectile
    private void OnEnable()
    {
        creationTime = Time.time;
        Invoke("Deactivate", life);
    }

    private void OnDisable()
    {
        CancelInvoke("Deactivate");

    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(-10f, -10f, -10f);
        GameManager.Instance.dataManager.RemoveDeactivatedProj(this);
    }

    // Damage Detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                if (!AOE)
                {
                    Debug.Log("Has taken damage");
                    monster.TakeDamage(Damage);
                    Deactivate();
                    GameManager.Instance.monsterManager.despawnCheck(monster);
                }
                else
                {
                    // Push to AOE list
                    GameManager.Instance.DamageExplosions.Add(new DamageExplosion(transform.position, DamageRange, Damage));
                    Deactivate();
                }
            }
        }
    }
}
