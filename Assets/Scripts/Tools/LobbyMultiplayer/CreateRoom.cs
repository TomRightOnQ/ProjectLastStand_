using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

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

    public void MainMenu() {
        DisconnectFromPhoton();
        SceneManager.LoadScene("MainMenu");
    }

    private void DisconnectFromPhoton()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
}
