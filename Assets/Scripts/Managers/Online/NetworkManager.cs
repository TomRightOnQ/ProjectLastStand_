using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

// Network Controlling
public class NetworkManager : MonoBehaviourPunCallbacks
{
    private static NetworkManager instance;

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NetworkManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Check repeated instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        print("Connecting...");
        Debug.Log(MasterManager.GameSettings);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName; 
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnApplicationQuit()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnConnectedToMaster()
    {
        print("Connected");
        print(PhotonNetwork.LocalPlayer.NickName);
        if (!PhotonNetwork.InLobby) {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected: " + cause.ToString());
    }
}
