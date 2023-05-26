using UnityEngine;
using Photon.Pun;

// Lasers
[RequireComponent(typeof(PhotonView))]
public class Lasers : Items, IPunObservable
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
    [SerializeField] private float pen = 0;
    [SerializeField] private bool aoe = false;
    [SerializeField] private float damageRange = 0.1f;
    [SerializeField] private int hitAnim = 0;
    [SerializeField] private bool isMagic = false;
    [SerializeField] private bool isNova = false;
    private float creationTime;

    // Sync
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      1. damage
        //      2. owner
        //      3. selfDet
        //      4. player
        //      5. pen
        //      6. aoe
        //      7. damageRange
        //      8. hitAnim
        //      9. isMagic
        //      10. isNova
        if (stream.IsWriting)
        {
            stream.SendNext(damage);
            stream.SendNext(owner);
            stream.SendNext(life);
            stream.SendNext(selfDet);
            stream.SendNext(player);
            stream.SendNext(pen);
            stream.SendNext(aoe);
            stream.SendNext(damageRange);
            stream.SendNext(hitAnim);
            stream.SendNext(isMagic);
            stream.SendNext(isNova);
        }
        else
        {
            damage = (float)stream.ReceiveNext();
            owner = (int)stream.ReceiveNext();
            life = (float)stream.ReceiveNext();
            selfDet = (bool)stream.ReceiveNext();
            player = (bool)stream.ReceiveNext();
            pen = (float)stream.ReceiveNext();
            aoe = (bool)stream.ReceiveNext();
            damageRange = (float)stream.ReceiveNext();
            hitAnim = (int)stream.ReceiveNext();
            isMagic = (bool)stream.ReceiveNext();
            isNova = (bool)stream.ReceiveNext();
        }
    }

    public float Damage { get { return damage; } set { damage = value; } }
    public int Owner { get { return owner; } set { owner = value; } }
    public float Life { get { return life; } set { life = value; } }
    public bool SelfDet { get { return selfDet; } set { selfDet = value; } }
    public bool Player { get { return player; } set { player = value; } }
    public float Pen { get { return pen; } set { pen = value; } }
    public bool AOE { get { return aoe; } set { aoe = value; } }
    public int HitAnim { get { return HitAnim; } set { hitAnim = value; } }
    public bool IsMagic { get { return isMagic; } set { isMagic = value; } }
    public bool IsNova { get { return isNova; } set { isNova = value; } }

    public float DamageRange
    {
        get { return damageRange; }
        set { damageRange = value; }
    }

    // Control the lifespan of a projectile
    public override void OnEnable()
    {
        creationTime = Time.time;
        Invoke("Deactivate", life);
    }

    public override void OnDisable()
    {
        CancelInvoke("Deactivate");
    }

    public void Activate()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("_activate", RpcTarget.All);
        }
        else
        {
            _activate();
        }
    }

    public void Deactivate()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("_deactivate", RpcTarget.All);
        }
        else
        {
            _deactivate();
        }
    }

    [PunRPC]
    private void _activate()
    {
        gameObject.SetActive(true);
    }

    // Dying
    [PunRPC]
    private void _deactivate()
    {
        transform.position = new Vector3(-10f, -10f, -10f);
        gameObject.SetActive(false);
    }

    // Damage Detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(hitAnim), Vector3.zero, Quaternion.identity);
                Vector3 pos = new Vector3(monster.transform.position.x, monster.transform.position.y + 4, monster.transform.position.z - 1.5f);
                animObject.transform.position = pos;
                animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
                animObject.transform.localScale = new Vector3(damageRange, damageRange, damageRange);
                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPCPlayHitAnim", RpcTarget.Others, hitAnim, pos, damageRange);
                }
                if (isNova)
                {
                    Explosions explosion = Instantiate(PrefabManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosions>();
                    explosion.Initialize(0.5f, damage / 3, pen, isMagic);
                }
                if (!AOE)
                {
                    monster.TakeDamage(damage, isMagic);
                    GameManager.Instance.monsterManager.despawnCheck(monster);
                }
                else
                {
                    // AOE
                    Explosions explosion = Instantiate(PrefabManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosions>();
                    explosion.Initialize(damageRange, damage, pen, isMagic);
                }
            }
        }
    }

    [PunRPC]
    public void RPCPlayHitAnim(int id, Vector3 pos, float scale)
    {
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(id), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = new Vector3(scale, scale, scale);
    }
}
