using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the entered multiplayer room
public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField] private PlayerListingsMenu playerListingsMenu;
    [SerializeField] private LeaveRoomMenu leaveRoomMenu;
    public LeaveRoomMenu LeaveRoomMenu { get { return leaveRoomMenu; } }
    private RoomsCanvases roomsCanvases;

    public void FirstInitialized(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
        playerListingsMenu.FirstInitialize(canvases);
        leaveRoomMenu.FirstInitialize(canvases);
    }

    public void Show() 
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
