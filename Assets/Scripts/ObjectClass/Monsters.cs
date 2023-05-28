using UnityEngine;
using Photon.Pun;
using static MonsterConfigs;


// All Monsters are one class
public class Monsters : Entities
{
    // Monster Stats
    [SerializeField] private float exp = 1;
    [SerializeField] private int id = 1;
    [SerializeField] private MonsterAI monsterAI;
    [SerializeField] private MonsterBehaviorType behaviorType;
    private float prevHP;

    public PrefabManager prefabManager;

    void Awake()
    {
        gameObject.tag = "Monster";
        prefabManager = Resources.Load<PrefabManager>("PrefabManager");
    }

    public float EXP { get { return exp; } set { exp = value; } }
    public int ID { get { return id; } set { id = value; } }
    public MonsterBehaviorType BehaviorType { get { return behaviorType; } }

    // Sync
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsWriting)
        {

        }
        else
        {
            UpdateHP();
        }
    }

    // Morph the Monster
    public void SetMonsters(MonsterConfig MonsterConfigs)
    {
        id = MonsterConfigs.id;
        name = MonsterConfigs._name;
        hitPoints = MonsterConfigs.hitPoints;
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
    }

    // Taking Damage
    public override void TakeDamage(float damage, bool isMagic)
    {
        base.TakeDamage(damage, isMagic);
        GameObject damageNumberObj = Instantiate(prefabManager.DamageNumberPrefab, transform.position, Quaternion.identity);
        DamageNumber damageNumber = damageNumberObj.GetComponent<DamageNumber>();
        damageNumber.Init(damage, transform.position, isMagic);
        if (PhotonNetwork.IsConnected) {
            //photonView.RPC("takeDamageRPC", RpcTarget.Others, transform.position, damage, isMagic);
        }
    }

    [PunRPC]
    public void takeDamageRPC(Vector3 position, float damage, bool isMagic)
    {
        GameObject damageNumberObj = Instantiate(prefabManager.DamageNumberPrefab, position, Quaternion.identity);
        DamageNumber damageNumber = damageNumberObj.GetComponent<DamageNumber>();
        damageNumber.Init(damage, position, isMagic); 
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

            // proj.Damage = (!isMagic) ? atk * playerAttack : atk * weaponAttack;

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
            // proj.AOE = aoe;
            proj.SwapMesh(ArtConfigs.Artconfig.DefaultMonsterProj);
            proj.GetComponent<Rigidbody>().velocity = direction * speed * 4;
        }
    }

    // HP Bar 
    public void UpdateHP()
    {
        if (prevHP != currentHitPoints) {
            float change = currentHitPoints - prevHP;

            prevHP = currentHitPoints;
        }
        hpS.maxValue = hitPoints;
        hpS.value = currentHitPoints;
    }

    // Update
    void Update()
    {
        UpdateHP();
        GameManager.Instance.monsterManager.despawnCheck(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            Base _base = other.gameObject.GetComponent<Base>();
            if (_base != null) {
                _base.TakeDamage(defaultAttack * defaultWeaponAttack, false);
            }
            GameManager.Instance.monsterManager.despawnForce(this);
        }
        else if (other.CompareTag("Player"))
        {
            Players _player = other.gameObject.GetComponent<Players>();
            if (_player != null)
            {
                _player.TakeDamage(defaultAttack * defaultWeaponAttack / 2, false);
            }
            GameManager.Instance.monsterManager.despawnForce(this);
        }
    }

    // Remove AI
    public override void Deactivate()
    {
        monsterAI.RemoveAI();
        base.Deactivate();
    }
}
