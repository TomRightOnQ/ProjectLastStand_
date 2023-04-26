using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the entered multiplayer room
public class CurrentRoomCanvas : MonoBehaviour
{
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
