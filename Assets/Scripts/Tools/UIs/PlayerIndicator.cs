using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

// Show the player on screen
public class PlayerIndicator : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Transform target;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private string nickNameText;
    [SerializeField] private TextMeshProUGUI nickName;

    private Quaternion INDICATOR_ROTATION;
    private Vector3 INDICATOR_POSITION;

    private RectTransform indicatorRectTransform;
    private Camera mainCamera;
    private Vector3 targetPosition;
    private Vector3 screenBoundsMin;
    private Vector3 screenBoundsMax;
    private bool ready = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(nickNameText);
        }
        else
        {
            nickNameText = (string)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        INDICATOR_ROTATION = Quaternion.Euler(45, 0, 0);
        INDICATOR_POSITION = new Vector3(0, 4.3f, 0);
    }

    public void SetUp(string _name)
    {
        if (indicatorRectTransform == null)
        {
            indicatorRectTransform = GetComponent<RectTransform>();
        }
        if (target == null) 
        {
            target = GetComponentInParent<Transform>();
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        nickNameText = _name;
        mainCamera = Camera.main;
        ready = true;
    }

    private void LateUpdate()
    {
        if (!ready) {
            return;
        }
        nickName.text = nickNameText;
        transform.rotation = INDICATOR_ROTATION;
        transform.localPosition = INDICATOR_POSITION;
    }
}