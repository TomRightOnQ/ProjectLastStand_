using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Leaving a room
public class LeaveRoomMenu : MonoBehaviour
{
    [SerializeField] private RoomsCanvases roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases) 
    {
        roomsCanvases = canvases;
    }

    public void OnClick_LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom(true);
        roomsCanvases.CurrentRoomCanvas.Hide();
        roomsCanvases.CreateOrJoinCanvas.Show();
    }
}
