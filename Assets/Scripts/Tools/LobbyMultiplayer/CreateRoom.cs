using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

// Create a new multiplayer room
public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private CreateOrJoinCanvas createOrJoinCanvas;
    [SerializeField] private CurrentRoomCanvas currentRoomCanvas;

    public void OnClickRoom() {
        if (!PhotonNetwork.IsConnected) {
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom() {
        Debug.Log("Created room successfully");
        createOrJoinCanvas.Hide();
        currentRoomCanvas.Show();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.LogWarning("Cretaed room Failed");
    }
}
