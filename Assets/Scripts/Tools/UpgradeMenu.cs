using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Menu that goes out each time the players reach to the enxt level
public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance { get; private set; }

    // For Displayer
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private Image arrow;
    private Vector3 originalPosition;
    private Vector3 closedPosition;
    private bool isOpen = true;
    private float panelWidth;
    [SerializeField] private float speed = 10;

    // For Data
    // Each points means one upgrade
    [SerializeField] private int points;
    public int Points { get { return points; } set { points = value; } }
    // Including three choices
    [SerializeField] private RectTransform upgradeChoices;
    // The annoying red dot
    [SerializeField] private Image redDot;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // For Display
        originalPosition = panelTransform.anchoredPosition;
        panelWidth = panelTransform.rect.width;
        closedPosition = new Vector3(-(panelWidth / 2), originalPosition.y, originalPosition.z);
    }

    void Update()
    {
        // For Display
        Vector3 targetPosition = isOpen ? originalPosition : closedPosition;
        float arrowZ = isOpen ? -90f : 90f;
        arrow.transform.rotation = Quaternion.Euler(arrow.transform.rotation.x, arrow.transform.rotation.y, arrowZ);
        panelTransform.anchoredPosition = Vector3.Lerp(panelTransform.anchoredPosition, targetPosition, speed * Time.deltaTime);
        if (points <= 0) {
            redDot.gameObject.SetActive(false);
        } else {
            redDot.gameObject.SetActive(true);
        }
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
    }
}
