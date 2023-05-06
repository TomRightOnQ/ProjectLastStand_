using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Hold the exp and levels info
// Only one copy per game
public class expAndLevels : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float exp = 0;
    [SerializeField] private float expL = 20;
    [SerializeField] private int level = 1;

    public float EXP { get { return exp; } set { exp = value; } }
    public float EXPL { get { return expL; } set { expL = value; } }
    public int Level { get { return level; } }

    public void Upgrade()
    {
        level += 1;
        exp = expL - exp;
        expL = 10 * (float)System.Math.Pow(1.1, level);
    }

    // Sync
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      1. exp
        //      2. level

        if (stream.IsWriting)
        {
            stream.SendNext(exp);
            stream.SendNext(level);

        }
        else
        {
            exp = (float)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
        }
    }
}
