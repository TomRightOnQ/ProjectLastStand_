using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static UpgradeConfigs;

// Menu that goes out each time the players reach to the enxt level
public class UpgradeMenu : MonoBehaviourPunCallbacks
{
    public static UpgradeMenu Instance { get; private set; }

    // For Display
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
    private bool regenChoices = false;

    // Including three choices
    [SerializeField] private Button choiceA;
    [SerializeField] private Button choiceB;
    [SerializeField] private Button choiceC;

    // The annoying red dot
    [SerializeField] private Image redDot;

    public int Points { get { return points; } set { points = value; } }

    void Awake()
    {
        gameObject.SetActive(true);
        Instance = this;
    }

    void Start()
    {
        // For Display
        originalPosition = panelTransform.anchoredPosition;
        panelWidth = panelTransform.rect.width;
        closedPosition = new Vector3(-(panelWidth / 2), originalPosition.y, originalPosition.z);
        enableAll();
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
        if (regenChoices == true) {
            generateUpgrade();
            regenChoices = false;
        }
        if (points > 0)
        {
            enableAll();
        }
        else {
            disableAll();
        }
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
    }

    // Methods on upgrades
    public void addPoints(int point)
    {
        regenChoices = false;
        if (points == 0) {
            regenChoices = true;
        }
        points += 1;
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
                generateUpgrade();
                isOpen = false;
            }
            else {
                regenChoices = true;
            }
        }
    }

    // Generate three upgrades
    private void generateUpgrade()
    {
        // Fill the text of each card
        fillUpgradeCard(choiceA);
        fillUpgradeCard(choiceB);
        fillUpgradeCard(choiceC);
    }

    private void fillUpgradeCard(Button card)
    {
        // 60% Upgrade, 40% Weapon
        // Select a random rarity level based on chances
        int chance = Random.Range(1, 101);
        int cardType = 1; // Get a upgrade
        UpgradeCards upCard = card.GetComponent<UpgradeCards>();
        upCard.IsWeapon = false;
        if (chance > 60)
        {
            upCard.IsWeapon = true;
            cardType = 2; // Get a weapon
        }

        switch (cardType)
        {
            case 1:
                UpgradeConfigs.UpgradeConfig upgrade = UpgradeConfigs.Instance.getUpgradeConfig();
                fillWithUpgrade(card, upgrade);
                break;
            case 2:
                WeaponConfigs.WeaponConfig weapon = WeaponConfigs.Instance.getWeaponConfig();
                fillWithWeapon(card, weapon);
                break;
        }
    }

    // Fill with upgrade
    private void fillWithUpgrade(Button card, UpgradeConfigs.UpgradeConfig upgradeConfig)
    {
        UpgradeCards upCard = card.GetComponent<UpgradeCards>();
        upCard.UpgradeData = upgradeConfig;
        upCard.fillUpgrade();
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

    // Fill with weapon
    private void fillWithWeapon(Button card, WeaponConfigs.WeaponConfig weaponConfig)
    {
        UpgradeCards upCard = card.GetComponent<UpgradeCards>();
        upCard.WeaponData = weaponConfig;
        upCard.fillWeapon();

        // Change the color of the card based on the rarity level
        Image cardImage = card.GetComponent<Image>();
        switch (weaponConfig.rating)
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

    // Choosing 
    public void choiceChosen(Button choice) {
        Players player = GameManager.Instance.GetLocalPlayer();
        if (player == null) {
            return;
        }
        UpgradeCards upCard = choice.GetComponent<UpgradeCards>();
        Debug.Log(upCard.IsWeapon);
        if (upCard.IsWeapon)
        {
            Debug.Log("Getting weapons");
            //read weaponConfig upCard.WeaponData
            WeaponConfigs.WeaponConfig weaponConfig = upCard.WeaponData;
            if (!PhotonNetwork.IsConnected)
            {
                player.addWeapon(0, weaponConfig.id);
            }
            else
            {
                player.addWeapon(0, weaponConfig.id);
                int playerViewID = GameManager.Instance.dataManager.PlayerViewID;
                int weaponID = weaponConfig.id;
                photonView.RPC("addWeaponsRPC", RpcTarget.Others, playerViewID, weaponID);
            }
        }
        else {
            //read upgradeConfig upCard.UpgradeData
            UpgradeConfigs.UpgradeConfig upgradeConfig = upCard.UpgradeData;
            player.HitPoints += upgradeConfig.hitPoints;
            player.CurrentHitPoints += upgradeConfig.regen;
            player.Speed += upgradeConfig.speed;
            player.DefaultAttack += upgradeConfig.defaultAttack;
            player.DefaultWeaponAttack += upgradeConfig.defaultWeaponAttack;
            player.DefaultDefence += upgradeConfig.defaultDefence;
            player.DefaultMagicDefence += upgradeConfig.defaultMagicDefence;
        }
    }

    [PunRPC]
    public void addWeaponsRPC(int playerViewID, int id) {
        Players player = GameManager.Instance.GetLocalPlayer(playerViewID);
        if (player == null)
        {
            return;
        }
        player.addWeapon(0, id);
    }

    public void AChosen() {
        choiceChosen(choiceA);
    }
    public void BChosen()
    {
        choiceChosen(choiceB);
    }
    public void CChosen()
    {
        choiceChosen(choiceC);
    }
}