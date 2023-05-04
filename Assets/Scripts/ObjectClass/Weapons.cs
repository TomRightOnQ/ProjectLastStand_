using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static ConfigManager;

// Define Weapons
public class Weapons : DefaultObjects
{
    // Weapon Stats
    [SerializeField] protected string wpName = "DeaultWeapon";
    [SerializeField] protected int id = 0;

    [SerializeField] protected int type = 0;
    [SerializeField] protected float attack = 10;
    [SerializeField] protected float pen = 0.1f;
    [SerializeField] protected float life = 6.0f;
    [SerializeField] protected float cd = 0.5f;
    [SerializeField] protected bool selfDet = false;
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] private float damageRange = 0.1f;
    [SerializeField] private bool aoe = false;

    public const string UPDATE_PROJ = "UpdatePosition";
    private float timer = 0;

    // Morph the weapon
    public void SetWeapons(WeaponConfig weaponConfigs)
    {
        id = weaponConfigs.id;
        wpName = weaponConfigs.name;
        type = weaponConfigs.type;
        attack = weaponConfigs.attack;
        pen = weaponConfigs.pen;
        life = weaponConfigs.life;
        cd = weaponConfigs.cd;
        selfDet = weaponConfigs.selfDet;
        projectileSpeed = weaponConfigs.projectileSpeed;
        aoe = weaponConfigs.aoe;
    }

    // Fire based on type
    public void Fire(int playerIdx, Vector3 direction)
    {
        if (timer < cd)
        {
            return;
        }
        switch (type) {
            case 0:
                FireBullet(playerIdx, direction);
                break;
            case 1:
                FireLaser(playerIdx, direction);
                break;
            case 2:
                FireLaser(playerIdx, direction);
                break;
        }
        timer = 0;
    }

    // Type 0: Bullet
    private void FireBullet(int playerIdx, Vector3 direction)
    {
        // Get projectile from pool
        Projectiles proj = GameManager.Instance.dataManager.TakeProjPool();
        if (proj != null)
        {
            int projectileID = proj.photonView.ViewID;
            Vector3 weaponForward = transform.TransformDirection(Vector3.forward);
            Vector3 firePos = transform.position + weaponForward * 0.5f;
            // Config the Projectile
            proj.transform.position = firePos;
            proj.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            proj.Damage = attack;
            proj.Owner = playerIdx;
            proj.Life = life;
            proj.SelfDet = true;
            proj.Player = true;
            proj.AOE = aoe;
            proj.GetComponent<Rigidbody>().velocity = direction * projectileSpeed * 10;
            proj.Activate();

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("SimulateProjectile", RpcTarget.Others, projectileID, photonView.ViewID, direction, projectileSpeed);
            }
        }
    }

    // Type 1: Laser
    private void FireLaser(int playerIdx, Vector3 direction)
    {
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
                    monster.TakeDamage(attack);
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
            lineRenderer.endColor = Color.red;
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
            proj.transform.position = localFirePos;
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
    }

    // Class properties
    public string WpName
    {
        get { return wpName; }
        set { wpName = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public int Type
    {
        get { return type; }
        set { type = value; }
    }

    public float Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public float Pen
    {
        get { return pen; }
        set { pen = value; }
    }

    public float Life
    {
        get { return life; }
        set { life = value; }
    }

    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
        set { projectileSpeed = value; }
    }

    public float CD
    {
        get { return cd; }
        set { cd = value; }
    }

    public bool SelfDet
    {
        get { return selfDet; }
        set { selfDet = value; }
    }

    public float DamageRange
    {
        get { return damageRange; }
        set { damageRange = value; }
    }
} 