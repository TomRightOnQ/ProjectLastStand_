using UnityEngine;

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
    private bool isMagic = false;

    public void Initialize(float damageRange, float damage, float pen, bool isMagic)
    {
        this.damageRange = damageRange;
        this.damage = Mathf.Ceil(damage);
        this.pen = pen;
        this.isMagic = isMagic;
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
            Debug.Log("Area Damage");
            Monsters monster = other.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                monster.TakeDamage(damage, isMagic);
            }
        }
    }
}
