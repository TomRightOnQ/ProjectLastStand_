using UnityEngine;
using Photon.Pun;
using System.Collections;

public class TacticalShield : Items
{
    private void Awake()
    {
        gameObject.tag = "Proj";
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proj"))
        {
            Projectiles projectile = other.gameObject.GetComponent<Projectiles>();
            if (projectile != null && projectile.Owner == -1)
            {
                if (PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPCReduceDamage", RpcTarget.All, projectile.photonView.ViewID);
                }
                else
                {
                    projectile.Damage -= projectile.Damage * 0.3f;
                }
            }
        }
    }


    [PunRPC]
    private void RPCReduceDamage(int projViewID)
    {
        Projectiles projectile = PhotonView.Find(projViewID)?.GetComponent<Projectiles>();
        if (projectile != null && projectile.Owner == -1)
        {
            projectile.Damage -= projectile.Damage * 0.3f;
        }
    }
}
