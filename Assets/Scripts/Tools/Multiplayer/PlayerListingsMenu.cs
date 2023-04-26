using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Show players in room
public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform content;
    [SerializeField] private PlayerListing playerListing;

    private List<PlayerListing> _listings = new List<PlayerListing>();

    private void Awake()
    {
        GerCurrentRoomPlayers();
    }

    private void GerCurrentRoomPlayers() 
    {
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
}
