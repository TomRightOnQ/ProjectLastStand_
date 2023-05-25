using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using static UpgradeConfigs;

// Entities are all NPCs and player-controlled units
public abstract class Entities : DefaultObjects, IPunObservable
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

    protected Effects _effect;
    protected List<EffectComponents> effects = new List<EffectComponents>();

    private void Awake()
    {
        _effect = GetComponent<Effects>();
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      1. hitPoints
        //      2. currentHitPoints
        //      3. defaultAttack
        //      4. defaultWeaponAttack
        //      5. defaultDefence
        //      6. defaultMagicDefence
        //      7. speed

        if (stream.IsWriting)
        {
            stream.SendNext(hitPoints);
            stream.SendNext(currentHitPoints);
            stream.SendNext(defaultAttack);
            stream.SendNext(defaultWeaponAttack);
            stream.SendNext(defaultDefence);
            stream.SendNext(defaultMagicDefence);
            stream.SendNext(speed);
        }
        else
        {
            hitPoints = (float)stream.ReceiveNext();
            currentHitPoints = (float)stream.ReceiveNext();
            defaultAttack = (float)stream.ReceiveNext();
            defaultWeaponAttack = (float)stream.ReceiveNext();
            defaultDefence = (float)stream.ReceiveNext();
            defaultMagicDefence = (float)stream.ReceiveNext();
            speed = (float)stream.ReceiveNext();
        }
    }

    // Getters and Setters
    public float HitPoints { get { return hitPoints; }set { hitPoints = value; } }
    public float CurrentHitPoints { get { return currentHitPoints; } set { currentHitPoints = value; } }
    public float DefaultAttack { get { return defaultAttack; } set { defaultAttack = value; } }
    public float DefaultWeaponAttack { get { return defaultWeaponAttack; } set { defaultWeaponAttack = value; } }
    public float DefaultDefence { get { return defaultDefence; } set { defaultDefence = value; } }
    public float DefaultMagicDefence { get { return defaultMagicDefence; } set { defaultMagicDefence = value; } }
    public float Speed { get { return speed; } set { speed = value; } }

    // Update
    void Update() {

    }

    // Add effects
    public virtual void AddEffect(int index, int level)
    {
        EffectComponents currentEffect = _effect.SetUp(index, level);
        if (currentEffect != null) {
            effects.Add(currentEffect);
        }
    }

    // Taking Damage
    public virtual void TakeDamage(float damage, bool isMagic) {

        currentHitPoints -= damage;
        if (currentHitPoints > hitPoints) {
            currentHitPoints = hitPoints;
        }
    }

    public void SwapMesh(int id) 
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            UpgradeConfig upgradeConfig = UpgradeConfigs.Instance._getUpgradeConfig(id);
            Mesh mesh = ArtConfigs.Instance.getMesh(upgradeConfig.mesh);
            // Call the RPC to synchronize the mesh change across the network
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

    [PunRPC]
    public void RPCSwapMesh(int id)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        UpgradeConfig upgradeConfig = UpgradeConfigs.Instance._getUpgradeConfig(id);
        Mesh mesh = ArtConfigs.Instance.getMesh(upgradeConfig.mesh);

        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }
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

    public virtual void Deactivate()
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
    protected void _activate()
    {
        gameObject.SetActive(true);
    }

    // Dying
    [PunRPC]
    protected void _deactivate()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(10f, -10f, -10f);
    }
}
