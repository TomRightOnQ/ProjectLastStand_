using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control the join room canvas
public class CreateOrJoinCanvas : MonoBehaviour
{
    [SerializeField] private CreateRoom createRoom;
    [SerializeField] private RoomListingsMenu roomListingsMenu;
    private RoomsCanvases roomsCanvases;

    public void FirstInitialized(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
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
