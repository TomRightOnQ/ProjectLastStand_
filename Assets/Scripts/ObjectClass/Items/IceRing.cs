using UnityEngine;
using Photon.Pun;
using System.Collections;

public class IceRing : Items
{
    private const float DAMAGE_INTERVAL = 0.5f;
    private float timer = 0f;

    void Awake()
    {
        gameObject.tag = "Proj";
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
    }

    // Damage Detection
    private void OnTriggerStay(Collider other)
    {
        if (timer <= DAMAGE_INTERVAL)
        {
            timer += Time.deltaTime;
            return;
        }
        if (other.CompareTag("Monster") && owner != -1)
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPCDamageToMonster", RpcTarget.All, monster.photonView.ViewID, damage, isMagic);
                }
                else
                {
                    monster.TakeDamage(damage, isMagic);
                }
                GameManager.Instance.monsterManager.despawnCheck(monster);
            }
        }
        timer = 0f;
    }

    // Slowdown
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && owner != -1)
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPCSlowMonster", RpcTarget.All, monster.photonView.ViewID, 0.3f);
                }
                else
                {
                    monster.Speed -= 0.3f;
                }
            }
        }
    }

    // Slowdown
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster") && owner != -1)
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPCSlowMonster", RpcTarget.All, monster.photonView.ViewID, -0.3f);
                }
                else
                {
                    monster.Speed -= 0.3f;
                }
            }
        }
    }

    [PunRPC]
    private void RPCDamageToMonster(int monsterViewID, float damage, bool isMagic)
    {
        Monsters monster = PhotonView.Find(monsterViewID).GetComponent<Monsters>();
        if (monster != null)
        {
            monster.TakeDamage(damage, isMagic);
            GameManager.Instance.monsterManager.despawnCheck(monster);
        }
    }

    [PunRPC]
    private void RPCSlowMonster(int monsterViewID, float value)
    {
        Monsters monster = PhotonView.Find(monsterViewID).GetComponent<Monsters>();
        if (monster != null)
        {
            monster.Speed -= value;
        }
    }
}
