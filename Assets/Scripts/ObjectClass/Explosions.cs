using UnityEngine;
using Photon.Pun;

// Explosion
public class Explosions : MonoBehaviourPun
{
    [SerializeField] private float damageRange;
    [SerializeField] private float damage;
    [SerializeField] private float pen;

    public float DamageRange { get { return damageRange; } set { damageRange = value; } }
    public float Damage { get { return damage; } set { damage = value; } }
    public float Pen { get { return pen; } set { pen = value; } }
    
    private bool initialized = false;
    private bool isMagic = false;
    private int hitAnim;
    private int owner = 0;

    public void Initialize(float _damageRange, float _damage, float _pen, bool _isMagic, int _owner, int _hitAnim)
    {
        damageRange = _damageRange;
        damage = Mathf.Ceil(_damage);
        pen = _pen;
        isMagic = _isMagic;
        owner = _owner;
        hitAnim = _hitAnim;
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
        if (other.CompareTag("Monster") && owner >= 0)
        {
            Monsters monster = other.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPCDamageToMonster", RpcTarget.All, monster.photonView.ViewID, damage, isMagic);
                }
                else 
                {
                    monster.TakeDamage(damage, isMagic,  pen);
                }
            }
        }
        if (other.CompareTag("Player") && owner == -1)
        {
            Players player = other.gameObject.GetComponent<Players>();
            if (player != null)
            {
                player.TakeDamage(damage, false, 0);
            }
        }
    }

    [PunRPC]
    private void RPCDamageToMonster(int monsterViewID, float damage, bool isMagic)
    {
        Monsters monster = PhotonView.Find(monsterViewID)?.GetComponent<Monsters>();
        if (monster != null)
        {
            monster.TakeDamage(damage, isMagic, pen);
            GameManager.Instance.monsterManager.despawnCheck(monster);
        }
    }

    public void PlayHitAnim(Vector3 pos)
    {
        if (AnimConfigs.Instance.GetAnim(hitAnim) == null)
            return;
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(hitAnim), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = new Vector3(damageRange, damageRange, damageRange);
    }

    [PunRPC]
    public void RPCPlayHitAnim(int id, Vector3 pos, float scale)
    {
        if (AnimConfigs.Instance.GetAnim(id) == null)
            return;
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(id), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = new Vector3(scale, scale, scale);
    }
}
