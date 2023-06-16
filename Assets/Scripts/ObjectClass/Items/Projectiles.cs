using UnityEngine;
using Photon.Pun;

// Bullets
[RequireComponent(typeof(PhotonView))]
public class Projectiles : Items
{
    private const string PREFAB_LOC = "Prefabs/";

    void Awake()
    {
        gameObject.tag = "Proj";
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
    }

    // Damage Detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && owner != -1)
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                Vector3 pos = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 4, other.gameObject.transform.position.z - 1.5f);
                AnimManager.Instance.PlayAnim(hitAnim, pos, new Vector3(damageRange, damageRange, damageRange));
                if (isNova)
                {
                    if (PhotonNetwork.IsConnected)
                    {
                        Explosions explosion = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.ExplosionPrefab.name, transform.position, Quaternion.identity).GetComponent<Explosions>();
                        explosion.Initialize(0.5f, damage / 3, pen, true, 0, hitAnim);
                    }
                    else {
                        Explosions explosion = Instantiate(PrefabManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosions>();
                        explosion.Initialize(0.5f, damage / 3, pen, true, 0, hitAnim);
                    }
                }
                if (!AOE)
                {
                    gameObject.SetActive(false);
                    Deactivate();
                    if (PhotonNetwork.IsConnected)
                    {
                        photonView.RPC("RPCDamageToMonster", RpcTarget.All, monster.photonView.ViewID, damage, isMagic);
                    }
                    else
                    {
                        monster.TakeDamage(damage, isMagic, pen);
                    }
                    GameManager.Instance.monsterManager.despawnCheck(monster);
                }
                else
                {
                    // AOE
                    if (!SelfDet)
                    {
                        if (PhotonNetwork.IsConnected)
                        {
                            Explosions explosion = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.ExplosionPrefab.name, transform.position, Quaternion.identity).GetComponent<Explosions>();
                            explosion.Initialize(damageRange, damage, pen, isMagic, 0, hitAnim);
                        }
                        else
                        {
                            Explosions explosion = Instantiate(PrefabManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosions>();
                            explosion.Initialize(damageRange, damage, pen, isMagic, 0, hitAnim);
                        }
                    }
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
                    _base.TakeDamage(damage, false, pen);
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
                player.TakeDamage(damage, false, pen);
                gameObject.SetActive(false);
                Deactivate();
            }
            gameObject.SetActive(false);
            Deactivate();
        }
    }

    [PunRPC]
    private void RPCDamageToMonster(int monsterViewID, float damage, bool isMagic)
    {
        Monsters monster = PhotonView.Find(monsterViewID).GetComponent<Monsters>();
        if (monster != null)
        {
            monster.TakeDamage(damage, isMagic, pen);
            GameManager.Instance.monsterManager.despawnCheck(monster);
        }
    }

    public override void OnEnable()
    {
        creationTime = Time.time;
        Invoke("Deactivate", life);
    }

    public override void OnDisable()
    {
        CancelInvoke("Deactivate");
        if (selfDet && aoe)
        {
            // AOE
            if (PhotonNetwork.IsConnected)
            {
                Explosions explosion = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.ExplosionPrefab.name, transform.position, Quaternion.identity).GetComponent<Explosions>();
                explosion.Initialize(damageRange, damage, pen, isMagic, 0, hitAnim);
            }
            else
            {
                Explosions explosion = Instantiate(PrefabManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosions>();
                explosion.Initialize(damageRange, damage, pen, isMagic, 0, hitAnim);
            }
            Vector3 pos = new Vector3(transform.position.x, gameObject.transform.position.y + 4, gameObject.transform.position.z - 1.5f);
            AnimManager.Instance.PlayAnim(hitAnim, pos, new Vector3(damageRange, damageRange, damageRange));
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCPlayHitAnim", RpcTarget.Others, hitAnim, pos, damageRange);
            }
        }
        else
        {
            // Regular disable behavior
            Deactivate();
        }
    }

    public void Activate()
    {
        if (!PhotonNetwork.IsConnected)
        {
            _activate();
        }
        else if (!photonView.IsMine)
        {
            return;
        }
        else if (photonView.IsMine)
        {
            photonView.RPC("_activate", RpcTarget.All);
        }
    }

    public void Deactivate()
    {
        AudioManager.Instance.PlaySound(hitSFX, transform.position);
        if (!PhotonNetwork.IsConnected)
        {
            _deactivate();
        }
        else if (!photonView.IsMine)
        {
            return;
        }
        else if (photonView.IsMine)
        {
            photonView.RPC("_deactivate", RpcTarget.All);
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
        transform.position = new Vector3(-10f, -20f, -20f);
        gameObject.SetActive(false);
    }
}
