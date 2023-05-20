using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
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
    
    private float atk = 2;
    public const string UPDATE_PROJ = "UpdatePosition";
    private float timer = 0;

    // Morph the weapon
    public void SetWeapons(WeaponConfig weaponConfigs)
    {
        id = weaponConfigs.id;
        wpName = weaponConfigs._name;
        rating = weaponConfigs.rating;
        type = weaponConfigs.type;
        attack = weaponConfigs.attack;
        pen = weaponConfigs.pen;
        life = weaponConfigs.life;
        cd = weaponConfigs.cd;
        selfDet = weaponConfigs.selfDet;
        projectileSpeed = weaponConfigs.projectileSpeed;
        aoe = weaponConfigs.aoe;
        info = weaponConfigs.info;
        intro = weaponConfigs.intro;
        hitAnim = weaponConfigs.hitAnim;
        damageRange = weaponConfigs.damageRange;
        atk = attack;
        SwapMesh(weaponConfigs.id);
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
        transform.LookAt(new Vector3 (direction.x, transform.position.y, direction.z));
        if (timer < cd)
        {
            return;
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
            proj.transform.position = new Vector3 (firePos.x, firePos.y, firePos.z);
            proj.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            proj.Damage = atk * playerAttack;
            proj.Owner = playerIdx;
            proj.Life = life;
            proj.SelfDet = true;
            proj.Player = true;
            proj.AOE = aoe;
            proj.HitAnim = hitAnim;
            proj.DamageRange = damageRange;
            proj.GetComponent<Rigidbody>().velocity = direction * projectileSpeed * 10;
            proj.Activate();

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("SimulateProjectile", RpcTarget.Others, projectileID, photonView.ViewID, direction, projectileSpeed);
            }
        }
    }

    // Type 1: Laser
    private void FireLaser(int playerIdx, float playerAttack, float weaponAttack)
    {
        Vector3 direction = transform.forward;
        RaycastHit[] hitInfos = Physics.RaycastAll(transform.position, direction, 800);
        Vector3 endPos = transform.position + direction * 5f;

        for (int i = 0; i < hitInfos.Length; i++)
        {
            RaycastHit hitInfo = hitInfos[i];

            if (hitInfo.collider.CompareTag("Monster"))
            {
                Monsters monster = hitInfo.collider.gameObject.GetComponent<Monsters>();
                if (monster != null)
                {
                    monster.TakeDamage(atk * playerAttack);
                    if (type == 2) {
                        endPos = hitInfo.point;
                        break;
                    }
                }
            }
            else if (hitInfo.collider.CompareTag("Base") || hitInfo.collider.CompareTag("Wall"))
            {
                endPos = hitInfo.point;
                break;
            }
        }
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("SimulateLaser", RpcTarget.All, photonView.ViewID, direction, endPos);
        }
        else {
            SimulateLaser(direction, endPos);
        }
    }

    private void SimulateLaser(Vector3 direction, Vector3 endPos)
    {
        Vector3 weaponForward = transform.TransformDirection(Vector3.forward);
        Vector3 startPos = transform.position + weaponForward * 0.5f;
        // Get or add the Line Renderer component to the weapon object
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            Color color = Color.red;
            color.a = 0.5f;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            lineRenderer.positionCount = 2;
        }

        // Update the Line Renderer with the laser positions
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        Destroy(lineRenderer, 0.05f);
    }

    // Simulating a laser
    [PunRPC]
    private void SimulateLaser(int weaponViewID, Vector3 direction, Vector3 endPos)
    {
        Weapons weapon = PhotonView.Find(weaponViewID).GetComponent<Weapons>();
        Vector3 weaponForward = weapon.transform.TransformDirection(Vector3.forward);
        Vector3 startPos = transform.position + weaponForward * 0.5f;
        // Get or add the Line Renderer component to the weapon object
        LineRenderer lineRenderer = weapon.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = weapon.gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            Color color = Color.red;
            color.a = 0.5f;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            lineRenderer.positionCount = 2;
        }

        // Update the Line Renderer with the laser positions
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        Destroy(lineRenderer, 0.05f);
    }

    // Simulating a projectile
    [PunRPC]
    private void SimulateProjectile(int projectileID, int weaponViewID, Vector3 direction, float speed)
    {
        // Get projectile from pool
        Projectiles proj = PhotonView.Find(projectileID).GetComponent<Projectiles>();
        Weapons weapon = PhotonView.Find(weaponViewID).GetComponent<Weapons>();

        if (proj != null)
        {
            Vector3 weaponForward = transform.TransformDirection(Vector3.forward);
            Vector3 localFirePos = transform.position + weaponForward * 0.5f;
            // Config the Projectile
            proj.transform.position = new Vector3(localFirePos.x, localFirePos.y, localFirePos.z) ;
            proj.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            proj.Damage = attack;
            proj.Owner = photonView.ViewID;
            proj.Life = life;
            proj.SelfDet = true;
            proj.Player = true;
            proj.AOE = aoe;
            proj.GetComponent<Rigidbody>().velocity = direction * speed * 10;
        }
    }

    public void SetRotation(Vector3 targetPosition)
    {
        transform.LookAt(targetPosition);
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
} 