using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UpgradeConfigs;

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
    [SerializeField] private TextMeshProUGUI availablePoints;

    // For Data
    // Each points means one upgrade
    [SerializeField] private int points = 0;
    private int prevPoints = 0;

    // Including three choices
    [SerializeField] private Button choiceA;
    [SerializeField] private Button choiceB;
    [SerializeField] private Button choiceC;

    // The annoying red dot
    [SerializeField] private Image redDot;

    public int Points { get { return points; } set { points = value; } }

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
        points += 1;
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
        availablePoints.text = "Available Upgrades: " + points.ToString();

        // For Data
        if (points != prevPoints && points > 0)
        {
            prevPoints = points;
            genetareUpgrade();
        }
        else if (points == 0 && prevPoints != 0) {
            prevPoints = points;
        }
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
    }

    // Methods on upgrades
    public void addPoints(int point)
    {
        points += 1;
        enableAll();
    }

    private void disableAll() 
    {
        choiceA.gameObject.SetActive(false);
        choiceB.gameObject.SetActive(false);
        choiceC.gameObject.SetActive(false);
    }

    private void enableAll()
    {
        choiceA.gameObject.SetActive(true);
        choiceB.gameObject.SetActive(true);
        choiceC.gameObject.SetActive(true);
    }

    public void pointsOff()
    {
        if (points > 0) {
            points -= 1;
            if (points == 0)
            {
                isOpen = false;
                disableAll();
            }
        }
    }

    // Generate three upgrades
    private void genetareUpgrade()
    {
        Debug.Log("Refeshing...");
        UpgradeConfigs.UpgradeConfig upgradeA = UpgradeConfigs.Instance.getUpgradeConfig();
        UpgradeConfigs.UpgradeConfig upgradeB = UpgradeConfigs.Instance.getUpgradeConfig();
        UpgradeConfigs.UpgradeConfig upgradeC = UpgradeConfigs.Instance.getUpgradeConfig();

        // Fill the text of each card
        fillUpgradeCard(choiceA, upgradeA);
        fillUpgradeCard(choiceB, upgradeB);
        fillUpgradeCard(choiceC, upgradeC);

        enableAll();
    }

    private void fillUpgradeCard(Button card, UpgradeConfigs.UpgradeConfig upgradeConfig)
    {
        // Get the text components of the card
        TextMeshProUGUI nameText = card.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI infoText = card.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        // Fill the name and info text
        nameText.text = upgradeConfig._name;
        infoText.text = upgradeConfig.description;

        // Change the color of the card based on the rarity level
        Image cardImage = card.GetComponent<Image>();
        switch (upgradeConfig.rating)
        {
            case 1: // white
                cardImage.color = Color.white;
                break;
            case 2: // green
                cardImage.color = Color.green;
                break;
            case 3: // blue
                cardImage.color = Color.blue;
                break;
            case 4: // purple
                cardImage.color = new Color(0.5f, 0f, 1f);
                break;
            case 5: // orange
                cardImage.color = new Color(1f, 0.5f, 0f);
                break;
            default:
                cardImage.color = Color.white;
                break;
        }
    }
}
