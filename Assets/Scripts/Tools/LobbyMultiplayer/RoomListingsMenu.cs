using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// List existing rooms
public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform content;
    [SerializeField] private RoomListing roomListing;
    [SerializeField] private CreateOrJoinCanvas createOrJoinCanvas;
    [SerializeField] private CurrentRoomCanvas currentRoomCanvas;

    private List<RoomListing> _listings = new List<RoomListing>();
    private RoomsCanvases roomsCanvases;

    public override void OnJoinedRoom()
    {
        createOrJoinCanvas.Hide();
        currentRoomCanvas.Show();
        content.DestryChildren();
        _listings.Clear();
    }
    public void FirstInitialize(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) 
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1) {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            else {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    RoomListing listing = (RoomListing)Instantiate(roomListing, content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
            }
        }
    }
}
