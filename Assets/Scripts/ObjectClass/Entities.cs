using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using static UpgradeConfigs;

// Entities are all NPCs and player-controlled units
public abstract class Entities : DefaultObjects, IPunObservable
{
    [SerializeField] protected float hitPoints = 20.0f;
    [SerializeField] protected float currentHitPoints = 20.0f;
    [SerializeField] protected float defaultAttack = 1.0f;
    [SerializeField] protected float defaultWeaponAttack = 1.0f;
    [SerializeField] protected float defaultDefence = 5.0f;
    [SerializeField] protected float defaultMagicDefence = 0;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float criticalRate = 0.2f;
    [SerializeField] protected float criticalDamage = 1.0f;
    [SerializeField] protected float criticalBase = 2.0f;
    [SerializeField] protected float criticalMod = 1.0f;
    [SerializeField] protected Slider hpS;

    [SerializeField] public float SpeedBase = 1.0f;
    [SerializeField] public float SpeedMod = 1.0f;
    [SerializeField] public float DamageBase = 1.0f;
    [SerializeField] public float WeaponDamageBase = 1.0f;
    [SerializeField] public float DamageMod = 1.0f;

    protected const float DAMAGE_COLOR_DURATION = 0.5f;
    protected float damageTimer = 0;
    protected bool isDamaged = false;

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
    public float CriticalRate { get { return criticalRate; } set { criticalRate = value; } }
    public float CriticalDamage { get { return criticalDamage; } set { criticalDamage = value; } }
    public float CriticalMod { get { return criticalMod; } set { criticalMod = value; } }
    public float CriticalBase { get { return criticalBase; } set { criticalBase = value; } }

    void Awake()
    {

    }

    // Taking Damage
    public virtual void TakeDamage(float damage, bool isMagic, float pen) 
    {
        AudioManager.Instance.PlaySound(12, transform.position);
        float minDamage = damage * 0.35f;
        if (isMagic)
        {
            damage = damage * (100 / (100 + defaultMagicDefence - (defaultMagicDefence * pen)));
        }
        else 
        {
            float effectiveDefense = defaultDefence - (defaultDefence * pen);

            if (effectiveDefense <= (damage * 0.5f))
            {
                damage = damage - effectiveDefense;
            }
            else
            {
                damage = damage - effectiveDefense - (100 / (100 + defaultDefence - (defaultDefence * pen)));
            }
            damage = Mathf.Max(minDamage, damage);
        }
        GameObject damageNumberObj = Instantiate(PrefabManager.Instance.DamageNumberPrefab, transform.position, Quaternion.identity);
        DamageNumber damageNumber = damageNumberObj.GetComponent<DamageNumber>();
        damageNumber.Init(damage, transform.position, isMagic);
        currentHitPoints -= damage;
        if (currentHitPoints > hitPoints) {
            currentHitPoints = hitPoints;
        }
        Rampage rampage = GetComponent<Rampage>();
        if (rampage != null) {
            rampage.Invoke();
        }
        damageTimer = 0.2f;
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
        if (meshFilter != null)
        {
            UpgradeConfig upgradeConfig = UpgradeConfigs.Instance._getUpgradeConfig(id);
            Mesh mesh = ArtConfigs.Instance.getMesh(upgradeConfig.mesh);
            meshFilter.mesh = mesh;
        }
    }

    public virtual void SwapMaterial(int id)
    {
        Material material = ArtConfigs.Instance.getMaterial(id);
        if (material != null)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSwapMaterial", RpcTarget.All, id);
            }
            else
            {
                GetComponent<Renderer>().material = material;
            }
        }
    }

    [PunRPC]
    public void RPCSwapMaterial(int id)
    {
        Material material = ArtConfigs.Instance.getMaterial(id);
        if (material != null)
        {
            {
                GetComponent<Renderer>().material = material;
            }
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

    protected virtual void Update()
    {
        if (damageTimer <= 0 && isDamaged)
        {
            isDamaged = false;
            SwapMaterial(0);
        } else if (damageTimer > 0 && !isDamaged) {
            isDamaged = true;
            SwapMaterial(2);
        }
        if (damageTimer > 0) 
        {
            damageTimer -= Time.deltaTime;
        }
    }
}
