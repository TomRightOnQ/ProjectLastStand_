using UnityEngine;
using Photon.Pun;
using TMPro;
using static MonsterConfigs;


// All Monsters are one class
public class Monsters : Entities
{
    // Monster Stats
    [SerializeField] protected float exp = 1;
    [SerializeField] protected int id = 1;
    [SerializeField] protected MonsterAI monsterAI;
    [SerializeField] protected MonsterBehaviorType behaviorType;
    public float prevHP;
    [SerializeField] public bool IsBoss = false;

    void Awake()
    {
        gameObject.tag = "Monster";
    }

    public float EXP { get { return exp; } set { exp = value; } }
    public int ID { get { return id; } set { id = value; } }
    public MonsterBehaviorType BehaviorType { get { return behaviorType; } }

    // Sync
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
    }

    // Morph the Monster
    public void SetMonsters(MonsterConfig MonsterConfigs, float diff)
    {
        id = MonsterConfigs.id;
        name = MonsterConfigs._name;
        hitPoints = MonsterConfigs.hitPoints * diff;
        currentHitPoints = hitPoints;
        speed = MonsterConfigs.speed;
        exp = MonsterConfigs.exp;
        defaultAttack = MonsterConfigs.defaultAttack;
        defaultWeaponAttack = MonsterConfigs.defaultWeaponAttack;
        defaultDefence = MonsterConfigs.defaultDefence;
        defaultMagicDefence = MonsterConfigs.defaultMagicDefence;
        prevHP = currentHitPoints;
        behaviorType = MonsterConfigs.behaviorType;
        monsterAI.SetUp();
        SwapMonsterMesh(MonsterConfigs.id);
    }

    public void SetEliteMonsters(MonsterConfig MonsterConfigs, float diff)
    {
        transform.localScale = new Vector3(1, 1, 1);
        id = MonsterConfigs.id;
        name = MonsterConfigs._name;
        hitPoints = MonsterConfigs.hitPoints * 5f;
        currentHitPoints = hitPoints;
        speed = MonsterConfigs.speed;
        exp = MonsterConfigs.exp * 2.5f;
        defaultAttack = MonsterConfigs.defaultAttack * 5f;
        defaultWeaponAttack = MonsterConfigs.defaultWeaponAttack * 1f;
        defaultDefence = 30f;
        defaultMagicDefence = 20f;
        prevHP = currentHitPoints;
        behaviorType = MonsterConfigs.behaviorType;
        monsterAI.SetUp();
        SwapMonsterMesh(MonsterConfigs.id);
    }

    public void SwapMonsterMesh(int id)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            MonsterConfig monsterConfig = MonsterConfigs.Instance.getMonsterConfig(id);
            Mesh mesh = ArtConfigs.Instance.getMesh(monsterConfig.mesh);
            // Call the RPC to synchronize the mesh change across the network
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSwapMonsterMesh", RpcTarget.All, id);
            }
            else
            {
                meshFilter.mesh = mesh;
            }
        }
    }

    [PunRPC]
    public void RPCSwapMonsterMesh(int id)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            MonsterConfig monsterConfig = MonsterConfigs.Instance.getMonsterConfig(id);
            Mesh mesh = ArtConfigs.Instance.getMesh(monsterConfig.mesh);
            meshFilter.mesh = mesh;
        }
    }

    // Taking Damage
    public override void TakeDamage(float damage, bool isMagic, float pen)
    {
        base.TakeDamage(damage, isMagic, pen);
    }

    // Fire
    public void FireBullet()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
            return;
        // Get projectile from pool
        Projectiles proj = GameManager.Instance.dataManager.MonsterTakeProjPool();
        if (proj != null)
        {
            int projectileID = proj.photonView.ViewID;
            Vector3 weaponForward = transform.TransformDirection(Vector3.forward);
            Vector3 firePos = transform.position + weaponForward * 0.5f;
            Vector3 direction = transform.forward;
            // Config the Projectile
            proj.transform.position = firePos;
            proj.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            proj.Damage = defaultAttack;
            proj.Owner = -1;
            proj.Life = 2;
            proj.SelfDet = true;
            proj.Player = true;
            proj.AOE = false;
            proj.HitAnim = 0;
            proj.DamageRange = 0.1f;
            proj.IsMagic = false;
            proj.GetComponent<Rigidbody>().velocity = direction * 10f * 4;
            proj.SwapMesh(ArtConfigs.Artconfig.DefaultMonsterProj);
            proj.Activate();

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSimulateProjectile", RpcTarget.Others, projectileID, direction, 10f);
            }
        }
    }

    // Simulating a projectile
    [PunRPC]
    public void RPCSimulateProjectile(int projectileID, Vector3 direction, float speed)
    {
        // Get projectile from pool
        Projectiles proj = PhotonView.Find(projectileID).GetComponent<Projectiles>();

        if (proj != null)
        {
            Vector3 weaponForward = transform.TransformDirection(Vector3.forward);
            Vector3 localFirePos = transform.position + weaponForward * 0.5f;
            // Config the Projectile
            proj.transform.position = localFirePos;
            proj.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            proj.Damage = defaultAttack * defaultWeaponAttack;
            proj.Owner = -1;
            proj.Life = 2f;
            proj.SelfDet = true;
            proj.Player = true;
            proj.hitSFX = -1;
            // proj.AOE = aoe;
            proj.SwapMesh(ArtConfigs.Artconfig.DefaultMonsterProj);
            proj.GetComponent<Rigidbody>().velocity = direction * speed * 4;
        }
    }

    // HP Bar 
    public virtual void UpdateHP()
    {
        if (prevHP != currentHitPoints) {
            float change = currentHitPoints - prevHP;

            prevHP = currentHitPoints;
        }
        hpS.maxValue = hitPoints;
        hpS.value = currentHitPoints;
    }

    // Update
    protected override void Update()
    {
        base.Update();
        UpdateHP();
        GameManager.Instance.monsterManager.despawnCheck(this);
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            Base _base = other.gameObject.GetComponent<Base>();
            if (_base != null) {
                _base.TakeDamage(defaultAttack * defaultWeaponAttack, false, 0);
            }
            GameManager.Instance.monsterManager.despawnForce(this);
        }
        else if (other.CompareTag("Player"))
        {
            Players _player = other.gameObject.GetComponent<Players>();
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z - 1.5f);
            AnimManager.Instance.PlayAnim(0, pos, new Vector3(1, 1, 1));
            if (_player != null)
            {
                _player.TakeDamage(defaultAttack * defaultWeaponAttack / 2, false, 0.5f);
            }
            GameManager.Instance.monsterManager.despawnForce(this);
        }
    }

    // Remove AI
    public override void Deactivate()
    {
        transform.position = new Vector3(-10f, -30f, 20f);
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        monsterAI.RemoveAI();
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPCUpdateMonster", RpcTarget.All, id);
        }
        else if (!PhotonNetwork.IsConnected)
        {
            PlayerListener.Instance.UpdateDict(id);
        }
        base.Deactivate();
    }

    [PunRPC]
    public virtual void RPCUpdateMonster(int id)
    {
        PlayerListener.Instance.UpdateDict(id);
    }
}
