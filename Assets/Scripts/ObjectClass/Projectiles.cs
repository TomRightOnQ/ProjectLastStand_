using UnityEngine;
using Photon.Pun;
using static WeaponConfigs;
using static ArtConfigs;

// Bullets
[RequireComponent(typeof(PhotonView))]
public class Projectiles : Items, IPunObservable
{
    void Awake() 
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

    public float DamageRange
    {
        get { return damageRange; }
        set { damageRange = value; }
    }

    public void SwapMesh(int id)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            WeaponConfig weaponConfig = WeaponConfigs.Instance._getWeaponConfig(id);
            Mesh mesh = ArtConfigs.Instance.getMesh(weaponConfig.projMesh);
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSwapMesh", RpcTarget.All, id);
            }
            else
            {
                meshFilter.mesh = mesh;
            }
        }
    }

    public void SwapMesh(Artconfig type)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Mesh mesh = ArtConfigs.Instance.getMesh(type);
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSwapMesh_Type", RpcTarget.All, type);
            }
            else
            {
                meshFilter.mesh = mesh;
            }
        }
    }

    [PunRPC]
    public void RPCSwapMesh(int id)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        WeaponConfig weaponConfig = WeaponConfigs.Instance._getWeaponConfig(id);
        Mesh mesh = ArtConfigs.Instance.getMesh(weaponConfig.projMesh);

        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }
    }

    [PunRPC]
    public void RPCSwapMesh_Type(Artconfig type)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = ArtConfigs.Instance.getMesh(type);

        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }
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
        if (other.CompareTag("Monster") && owner != -1)
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                Vector3 pos = new Vector3(monster.transform.position.x, monster.transform.position.y + 4, monster.transform.position.z - 1.5f);
                PlayHitAnim(pos);
                if (PhotonNetwork.IsConnected) {
                    photonView.RPC("RPCPlayHitAnim", RpcTarget.Others, hitAnim, pos, damageRange);
                }

                if (!AOE)
                {
                    gameObject.SetActive(false);
                    Deactivate();
                    monster.TakeDamage(damage, isMagic);
                    GameManager.Instance.monsterManager.despawnCheck(monster);
                }
                else
                {
                    Debug.Log("Area Damage");
                    // AOE
                    Explosions explosion = Instantiate(PrefabManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosions>();
                    explosion.Initialize(damageRange, damage, pen, isMagic);
                    gameObject.SetActive(false);
                    Deactivate();
                }
            }
        }
        else if (other.CompareTag("Base"))
        {
            if (owner == -1) {
                Base _base = other.gameObject.GetComponent<Base>();
                if (_base != null)
                {
                    _base.TakeDamage(damage, false);
                    gameObject.SetActive(false);
                    Deactivate();
                }
            }
            gameObject.SetActive(false);
            Deactivate();
        }
        else if (other.CompareTag("Player") && owner == -1)
        {
            Players player = other.gameObject.GetComponent<Players>();
            if (player != null)
            {
                player.TakeDamage(damage, false);
                gameObject.SetActive(false);
                Deactivate();
            }
            gameObject.SetActive(false);
            Deactivate();
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
