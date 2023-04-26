using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private Text text;

    public RoomInfo RoomInfo { get; private set; }
    public void SetRoomInfo(RoomInfo roomInfo) 
    {
        RoomInfo = roomInfo;
        text.text = roomInfo.Name + ": " +roomInfo.MaxPlayers;
    }

    public void OnClick_Button() {
        Debug.Log("Joining...");
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
