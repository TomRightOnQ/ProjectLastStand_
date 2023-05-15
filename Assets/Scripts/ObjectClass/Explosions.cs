using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

// Explosion
public class Explosions : MonoBehaviour
{
    [SerializeField] private float damageRange;
    [SerializeField] private float damage;
    [SerializeField] private float pen;

    public float DamageRange { get { return damageRange; } set { damageRange = value; } }
    public float Damage { get { return damage; } set { damage = value; } }
    public float Pen { get { return pen; } set { pen = value; } }

    private bool initialized = false;

    public void Initialize(float damageRange, float damage, float pen)
    {
        this.damageRange = damageRange;
        this.damage = damage;
        this.pen = pen;
        GetComponent<SphereCollider>().enabled = true;
        initialized = true;
    }

    private void Update()
    {
        if (initialized)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void Awake()
    {
        // Disable the collider initially
        GetComponent<SphereCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Monsters monster = other.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                monster.TakeDamage(damage);
            }
        }
    }
}
