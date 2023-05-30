using UnityEngine;
using Photon.Pun;
using static WeaponConfigs;
using static ArtConfigs;

public class Items : DefaultObjects, IPunObservable
{
    // Projectile stats
    [SerializeField] protected float damage = 1;
    [SerializeField] protected int owner = 0;
    [SerializeField] protected float life = 1.0f;
    [SerializeField] protected bool selfDet = false;
    [SerializeField] protected bool player = false;
    [SerializeField] protected float pen = 0;
    [SerializeField] protected bool aoe = false;
    [SerializeField] protected float damageRange = 0.1f;
    [SerializeField] protected int hitAnim = 0;
    [SerializeField] protected bool isMagic = false;
    [SerializeField] protected bool isNova = false;
    protected float creationTime;

    // Sync
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
    public float DamageRange { get { return damageRange; } set { damageRange = value; } }

    public void SwapMesh(int id)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            WeaponConfig weaponConfig = WeaponConfigs.Instance._getWeaponConfig(id);
            Mesh mesh = ArtConfigs.Instance.getMesh(weaponConfig.projMesh);
            Material material = ArtConfigs.Instance.getMaterial(weaponConfig.projMesh);
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSwapMesh", RpcTarget.All, id);
            }
            else
            {
                GetComponent<Renderer>().material = material;
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
            Material material = ArtConfigs.Instance.getMaterial(type);
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSwapMesh_Type", RpcTarget.All, type);
            }
            else
            {
                GetComponent<Renderer>().material = material;
                meshFilter.mesh = mesh;
            }
        }
    }

    [PunRPC]
    public virtual void RPCSwapMesh(int id)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        WeaponConfig weaponConfig = WeaponConfigs.Instance._getWeaponConfig(id);
        Mesh mesh = ArtConfigs.Instance.getMesh(weaponConfig.projMesh);
        Material material = ArtConfigs.Instance.getMaterial(weaponConfig.projMesh);
        if (meshFilter != null)
        {
            GetComponent<Renderer>().material = material;
            meshFilter.mesh = mesh;
        }
    }

    [PunRPC]
    public virtual void RPCSwapMesh_Type(Artconfig type)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = ArtConfigs.Instance.getMesh(type);
        Material material = ArtConfigs.Instance.getMaterial(type);
        if (meshFilter != null)
        {
            GetComponent<Renderer>().material = material;
            meshFilter.mesh = mesh;
        }
    }
}
