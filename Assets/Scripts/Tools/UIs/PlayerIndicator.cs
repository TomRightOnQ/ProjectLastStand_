using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Show the player on screen
public class PlayerIndicator : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform target;
    [SerializeField] private Image indicatorImage;

    private Quaternion INDICATOR_ROTATION;
    private Vector3 INDICATOR_POSITION;

    private RectTransform indicatorRectTransform;
    private Camera mainCamera;
    private Vector3 targetPosition;
    private Vector3 screenBoundsMin;
    private Vector3 screenBoundsMax;
    private bool ready = false;

    private void Awake()
    {
        INDICATOR_ROTATION = Quaternion.Euler(45, 0, 0);
        INDICATOR_POSITION = new Vector3(0, 4.3f, 0);
    }

    public void SetUp()
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

        mainCamera = Camera.main;
        ready = true;
    }

    private void LateUpdate()
    {
        if (!ready) {
            return;
        }

        transform.rotation = INDICATOR_ROTATION;
        transform.localPosition = INDICATOR_POSITION;
    }
}