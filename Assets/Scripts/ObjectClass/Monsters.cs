using UnityEngine;
using Photon.Pun;
using TMPro;
using static MonsterConfigs;


// All Monsters are one class
public class Monsters : Entities
{
    // Monster Stats
    [SerializeField] private float exp = 1;
    [SerializeField] private int id = 1;
    [SerializeField] private MonsterAI monsterAI;
    [SerializeField] private MonsterBehaviorType behaviorType;
    [SerializeField] private TextMeshProUGUI nameText;
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
        nameText.text = name;
        SwapMonsterMesh(MonsterConfigs.id);
    }

    public void SetEliteMonsters(MonsterConfig MonsterConfigs)
    {
        transform.localScale = new Vector3(1, 1, 1);
        id = MonsterConfigs.id;
        name = MonsterConfigs._name;
        hitPoints = MonsterConfigs.hitPoints * 2.5f;
        currentHitPoints = hitPoints;
        speed = MonsterConfigs.speed;
        exp = MonsterConfigs.exp * 2;
        defaultAttack = MonsterConfigs.defaultAttack * 1.5f;
        defaultWeaponAttack = MonsterConfigs.defaultWeaponAttack * 1.5f;
        defaultDefence = MonsterConfigs.defaultDefence;
        defaultMagicDefence = MonsterConfigs.defaultMagicDefence;
        prevHP = currentHitPoints;
        behaviorType = MonsterConfigs.behaviorType;
        monsterAI.SetUp();
        nameText.text = name;
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
    public override void TakeDamage(float damage, bool isMagic)
    {
        base.TakeDamage(damage, isMagic);
        GameObject damageNumberObj = Instantiate(prefabManager.DamageNumberPrefab, transform.position, Quaternion.identity);
        DamageNumber damageNumber = damageNumberObj.GetComponent<DamageNumber>();
        damageNumber.Init(damage, transform.position, isMagic);
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
    protected override void Update()
    {
        base.Update();
        UpdateHP();
        GameManager.Instance.monsterManager.despawnCheck(this);
    }

    public void PlayHitAnim(Vector3 pos)
    {
        if (AnimConfigs.Instance.GetAnim(0) == null)
            return;
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(0), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = new Vector3(1f, 1f, 1f);
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
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z - 1.5f);
            PlayHitAnim(pos);
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCPlayHitAnim", RpcTarget.Others, 0, pos, 1f);
            }
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
    public void RPCUpdateMonster(int id)
    {
        PlayerListener.Instance.UpdateDict(id);
    }
}
