using System;
using UnityEngine;
using Photon.Pun;
using static WeaponConfigs;

// Define Weapons
public class Weapons : DefaultObjects
{
    // Weapon Stats
    [SerializeField] private string wpName = "DeaultWeapon";
    [SerializeField] private int id = 0;
    [SerializeField] private int rating = 1;
    [SerializeField] private int type = 0;
    [SerializeField] private float extraAttack = 0;
    [SerializeField] private float attack = 2;
    [SerializeField] private float pen = 0.1f;
    [SerializeField] private float life = 6.0f;
    [SerializeField] private float cd = 0.5f;
    [SerializeField] private bool selfDet = false;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float damageRange = 0.1f;
    [SerializeField] private bool aoe = false;
    [SerializeField] private string info = "";
    [SerializeField] private string intro = "";
    [SerializeField] private int level = 1;
    [SerializeField] private int hitAnim = 0;
    [SerializeField] private int fireAnim = 0;
    [SerializeField] private bool isMagic;

    private const float LASER_LENGTH = 200f;
    private const float PROJ_YOFFSET = 1.1f;
    private const float LASER_YOFFSET = 1.6f;

    private float atk = 2;
    private float timer = 0;

    // Morph the weapon
    public void SetWeapons(WeaponConfig weaponConfig)
    {
        id = weaponConfig.id;
        wpName = weaponConfig._name;
        rating = weaponConfig.rating;
        type = weaponConfig.type;
        attack = weaponConfig.attack;
        pen = weaponConfig.pen;
        life = weaponConfig.life;
        cd = weaponConfig.cd;
        selfDet = weaponConfig.selfDet;
        projectileSpeed = weaponConfig.projectileSpeed;
        aoe = weaponConfig.aoe;
        info = weaponConfig.info;
        intro = weaponConfig.intro;
        hitAnim = weaponConfig.hitAnim;
        fireAnim = weaponConfig.fireAnim;
        damageRange = weaponConfig.damageRange;
        isMagic = weaponConfig.isMagic;
        atk = attack;
        SwapMesh(weaponConfig.id);
        level = 1;
    }

    public void SwapMesh(int id)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            WeaponConfig weaponConfig = WeaponConfigs.Instance._getWeaponConfig(id);
            Mesh mesh = ArtConfigs.Instance.getMesh(weaponConfig.mesh);
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
        WeaponConfig weaponConfig = WeaponConfigs.Instance._getWeaponConfig(id);
        Mesh mesh = ArtConfigs.Instance.getMesh(weaponConfig.mesh);

        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }
    }

    private void Start()
    {
        atk = attack + extraAttack;
    }

    // Updrage
    public void Upgrade(int _level) 
    {
        level += _level;
        configureLevel();
    }

    // Change weapon based on the current level;
    private void configureLevel()
    {
        extraAttack = (level - 1) * MathF.Ceiling((0.1f * attack));
        atk = attack + extraAttack;
    }

    // Fire based on type
    public void Fire(int playerIdx, Vector3 direction, float playerAttack, float weaponAttack)
    {
        if (timer < cd)
        {
            return;
        }
        Vector3 firePos = transform.position + transform.TransformDirection(Vector3.forward) * 1f;
        PlayFireAnim(firePos);
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPCPlayFireAnim", RpcTarget.Others, hitAnim, firePos, damageRange);
        }
        switch (type) {
            case 0:
                FireBullet(playerIdx, playerAttack, weaponAttack);
                break;
            case 1:
                FireLaser(playerIdx, playerAttack, weaponAttack);
                break;
            case 2:
                FireLaser(playerIdx, playerAttack, weaponAttack);
                break;
        }
        timer = 0;
    }

    // Type 0: Bullet
    private void FireBullet(int playerIdx, float playerAttack, float weaponAttack)
    {
        // Get projectile from pool
        Projectiles proj = GameManager.Instance.dataManager.TakeProjPool();
        if (proj != null)
        {
            int projectileID = proj.photonView.ViewID;
            Vector3 weaponForward = transform.TransformDirection(Vector3.forward);
            Vector3 firePos = transform.position + weaponForward * 0.5f;
            Vector3 direction = transform.forward;
            // Config the Projectile
            proj.transform.position = firePos;
            proj.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            proj.Damage = (!isMagic) ? atk * playerAttack : atk * weaponAttack;

            proj.Owner = playerIdx;
            proj.Life = life;
            proj.SelfDet = true;
            proj.Player = true;
            proj.AOE = aoe;
            proj.HitAnim = hitAnim;
            proj.DamageRange = damageRange;
            proj.IsMagic = isMagic;
            proj.GetComponent<Rigidbody>().velocity = direction * projectileSpeed * 10;
            proj.SwapMesh(id);
            proj.Activate();

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSimulateProjectile", RpcTarget.Others, projectileID, photonView.ViewID, direction, projectileSpeed);
            }
        }
    }

    // Type 1/2: Laser
    private void FireLaser(int playerIdx, float playerAttack, float weaponAttack)
    {
        // Get laser from pool
        Lasers laser = GameManager.Instance.dataManager.TakeLaserPool();
        if (laser != null)
        {
            int laserID = laser.photonView.ViewID;
            Vector3 weaponForward = transform.TransformDirection(Vector3.forward);
            Vector3 firePos = transform.position + weaponForward * 0.5f + weaponForward * LASER_LENGTH;
            Vector3 simulatePos = transform.position + weaponForward * 0.5f;
            Vector3 direction = transform.forward;

            // Config the Line Renderer
            LineRenderer lineRenderer = laser.GetComponent<LineRenderer>();

            // Config the Projectile
            laser.transform.localPosition = new Vector3(firePos.x, LASER_YOFFSET, firePos.z);
            laser.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            laser.Damage = (!isMagic) ? atk * playerAttack : atk * weaponAttack;
            laser.Owner = playerIdx;
            laser.Life = life;
            laser.SelfDet = true;
            laser.Player = true;
            laser.AOE = aoe;
            laser.HitAnim = hitAnim;
            laser.DamageRange = damageRange;
            laser.IsMagic = isMagic;
            laser.Activate();

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSimulateLaser", RpcTarget.Others, laserID, simulatePos, simulatePos + direction * LASER_LENGTH / 2);
            }
            SimulateLaser(lineRenderer, simulatePos, simulatePos + direction * LASER_LENGTH / 2);
        }
    }

    private void SimulateLaser(LineRenderer lineRenderer, Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    // Simulating a laser
    [PunRPC]
    private void RPCSimulateLaser(int laserViewID, Vector3 start, Vector3 end)
    {
        // Get the laser object
        Lasers laser = PhotonView.Find(laserViewID).GetComponent<Lasers>();
        if (laser != null)
        {
            // Config the Line Renderer
            LineRenderer lineRenderer = laser.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }
    }

    // Simulating a projectile
    [PunRPC]
    private void RPCSimulateProjectile(int projectileID, int weaponViewID, Vector3 direction, float speed)
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
            proj.Damage = attack;
            proj.Owner = photonView.ViewID;
            proj.Life = life;
            proj.SelfDet = true;
            proj.Player = true;
            proj.AOE = aoe;
            proj.SwapMesh(id);
            proj.GetComponent<Rigidbody>().velocity = direction * speed * 10;
        }
    }

    public void PlayFireAnim(Vector3 pos)
    {
        if (AnimConfigs.Instance.GetAnim(fireAnim) == null)
            return;
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(fireAnim), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = new Vector3(damageRange, damageRange, damageRange);
    }

    [PunRPC]
    public void RPCPlayFireAnim(int id, Vector3 pos, float scale)
    {
        if (AnimConfigs.Instance.GetAnim(id) == null)
            return;
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(id), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.localRotation = rotation;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        atk = attack + extraAttack;
    }

    // Class properties
    public int Level { get { return level; } set { level = value; } }
    public string WpName { get { return wpName; } set { wpName = value; } }
    public int ID { get { return id; } set { id = value; } }
    public int Rating { get { return rating; } set { rating = value; } }
    public int Type { get { return type; } set { type = value; } }
    public float Attack { get { return attack; } set { attack = value; } }
    public float Pen { get { return pen; } set { pen = value; } }
    public float Life { get { return life; } set { life = value; } }
    public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }
    public float CD { get { return cd; } set { cd = value; } }
    public bool SelfDet { get { return selfDet; } set { selfDet = value; } }
    public float DamageRange { get { return damageRange; } set { damageRange = value; } }
    public string Info { get { return info; } set { Info = value; } }
    public string Intro { get { return intro; } set { Intro = value; } }
    public float Atk { get { return atk; } }
    public int HitAnim { get { return HitAnim; } }
    public int FireAnim { get { return fireAnim; } }
    public bool IsMagic { get { return isMagic; } set { isMagic = value; } }
} 