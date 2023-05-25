using UnityEngine;


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
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
