using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Reference of canvases
public class RoomsCanvases : MonoBehaviour
{
    [SerializeField] private CreateOrJoinCanvas createOrJoinCanvas;
    [SerializeField] private CurrentRoomCanvas currentRoomCanvas;

    public CreateOrJoinCanvas CreateOrJoinCanvas { get { return createOrJoinCanvas; } }
    public CurrentRoomCanvas CurrentRoomCanvas { get { return currentRoomCanvas; } }

    private void Awake()
    {
        FirstInitialize();
    }
    private void FirstInitialize()
    {
        CreateOrJoinCanvas.FirstInitialized(this);
        CurrentRoomCanvas.FirstInitialized(this);
    }
}
