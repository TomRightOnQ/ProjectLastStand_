using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

// Show players in room
public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform content;
    [SerializeField] private PlayerListing playerListing;
    [SerializeField] private RoomsCanvases roomsCanvases;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private Button startButton;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private bool ready = false;

    private const string RPC_READY = "RPC_ChangeReadyState";

    private void Awake()
    {
        GerCurrentRoomPlayers();
        if (!PhotonNetwork.IsMasterClient)
        {
            readyText.text = "Ready!";
        }
        else {
            startButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyText.text = "Start Game";
            if (AllReady())
            {
                startButton.gameObject.SetActive(true);
            }
            else {
                startButton.gameObject.SetActive(false);
            }
        }
        else {
            if (!ready)
                readyText.text = "READY!";
            else
                readyText.text = "AWATTING...";
        }
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
    }

    public override void OnLeftRoom()
    {
        content.DestryChildren();
    }

    private void GerCurrentRoomPlayers() 
    {
        if (!PhotonNetwork.IsConnected) {
            return;
        }
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null) {
            return;
        }
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players) {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player) {
        PlayerListing listing = (PlayerListing)Instantiate(playerListing, content);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            _listings.Add(listing);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    private bool AllReady() 
    {
        for (int i = 0; i < _listings.Count; i++)
        {
            if (_listings[i].Player != PhotonNetwork.LocalPlayer)
            {
                if (!_listings[i].Ready) { return false; }
            }
        }
        return true;
    }

    public void OnClick_StartGame() 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!AllReady()) { return; }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
        else {
            if (ready)
            {
                ready = false;
                readyText.text = "READY";
                base.photonView.RPC(RPC_READY, RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, ready);
            }
            else
            {
                ready = true;
                readyText.text = "AWAITING...";
                base.photonView.RPC(RPC_READY, RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, ready);
            }
        }
    }

    // Indicating player ready
    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool _ready)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listings[index].Ready = _ready;
        }
    }
}
