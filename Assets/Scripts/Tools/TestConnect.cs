using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        print("Connecting...");
        PhotonNetwork.GameVersion = "Dev 0.2";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        print("Connected");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected: " + cause.ToString());
    }
}
