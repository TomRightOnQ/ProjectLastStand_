using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using static WeaponConfigs;

// Everything except playuers and NPCs
public class Items : DefaultObjects, IPunObservable
{
    // Display
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI leveltext;
    [SerializeField] TextMeshProUGUI _name;

    // Data
    private int weaponIndex;
    private int level;
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      1. weaponIndex
        //      2. level

        if (stream.IsWriting)
        {
            stream.SendNext(weaponIndex);
            stream.SendNext(level);
        }
        else
        {
            weaponIndex = (int)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
        }
    }
}
