using UnityEngine;
using Photon.Pun;

// Explosion
public class Explosions : MonoBehaviourPun, IPunObservable
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

    void Awake()
    {
        GetComponent<SphereCollider>().enabled = false;
    }

    public void Initialize(float _damageRange, float _damage, float _pen, bool _isMagic, int _owner, int _hitAnim)
    {
        damageRange = _damageRange;
        damage = Mathf.Ceil(_damage);
        pen = _pen;
        isMagic = _isMagic;
        owner = _owner;
        hitAnim = _hitAnim;
        initialized = true;
        GetComponent<SphereCollider>().enabled = true;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(damageRange);
            stream.SendNext(damage);
            stream.SendNext(pen);
            stream.SendNext(isMagic);
            stream.SendNext(owner);
            stream.SendNext(hitAnim);
            stream.SendNext(initialized);
        }
        else
        {
            damageRange = (float)stream.ReceiveNext();
            damage = (float)stream.ReceiveNext();
            pen = (float)stream.ReceiveNext();
            isMagic = (bool)stream.ReceiveNext();
            owner = (int)stream.ReceiveNext();
            hitAnim = (int)stream.ReceiveNext();
            initialized = (bool)stream.ReceiveNext();
            GetComponent<SphereCollider>().enabled = true;
        }
    }

    private void Update()
    {
        if (initialized)
        {
            Destroy(gameObject, 0.1f);
        }
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
                    photonView.RPC("RPCDamageToMonster", RpcTarget.All, monster.photonView.ViewID, damage, isMagic, pen);
                }
                else 
                {
                    monster.TakeDamage(damage, isMagic, pen);
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
    private void RPCDamageToMonster(int monsterViewID, float damage, bool isMagic, float pen)
    {
        Monsters monster = PhotonView.Find(monsterViewID)?.GetComponent<Monsters>();
        if (monster != null)
        {
            monster.TakeDamage(damage, isMagic, pen);
            GameManager.Instance.monsterManager.despawnCheck(monster);
        }
    }
}
