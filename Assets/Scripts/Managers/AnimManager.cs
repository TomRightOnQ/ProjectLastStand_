using UnityEngine;
using Photon.Pun;
using static AnimConfigs;

// Anim Playing
public class AnimManager : MonoBehaviourPunCallbacks
{
    public static AnimManager Instance;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void PlayAnim(int id, Vector3 pos, Vector3 scale)
    {
        if (AnimConfigs.Instance.GetAnim(id) == null)
        {
            return;
        }
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(id), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = scale;
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPCPlayAnim", RpcTarget.Others, id, pos, scale);
        }
    }

    [PunRPC]
    public void RPCPlayAnim(int id, Vector3 pos, Vector3 scale)
    {
        if (AnimConfigs.Instance.GetAnim(id) == null)
        {
            return;
        }
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(id), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = scale;
    }
}
