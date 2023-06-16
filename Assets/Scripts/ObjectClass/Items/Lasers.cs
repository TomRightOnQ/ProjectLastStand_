using UnityEngine;
using Photon.Pun;
using static ArtConfigs;

// Lasers
[RequireComponent(typeof(PhotonView))]
public class Lasers : Items, IPunObservable
{
    void Awake()
    {
        gameObject.tag = "Proj";
    }

    // Sync
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
    }

    // Damage Detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                Vector3 pos = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 4, other.gameObject.transform.position.z - 1.5f);
                AnimManager.Instance.PlayAnim(hitAnim, pos, new Vector3(damageRange, damageRange, damageRange));
                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPCDamageToMonster", RpcTarget.All, monster.photonView.ViewID, damage, isMagic);
                }
                else {
                    monster.TakeDamage(damage, isMagic, pen);
                }
                GameManager.Instance.monsterManager.despawnCheck(monster);
            }
        }
    }

    [PunRPC]
    private void RPCDamageToMonster(int monsterViewID, float damage, bool isMagic)
    {
        Monsters monster = PhotonView.Find(monsterViewID)?.GetComponent<Monsters>();
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
        transform.position = new Vector3(-10f, -60f, -20f);
        gameObject.SetActive(false);
    }
}
