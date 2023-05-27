using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

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
    private const string PREFAB_LOC = "Prefabs/";

    // Including three choices
    [SerializeField] private Button expandBtn;
    [SerializeField] private Button choiceA;
    [SerializeField] private Button choiceB;
    [SerializeField] private Button choiceC;
    private Button[] choiceArr = new Button[3];

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
        choiceArr[0] = choiceA;
        choiceArr[1] = choiceB;
        choiceArr[2] = choiceC;
        regenChoices = false;
        InitUpgradeCard();
        enableAll();
    }

    void Update()
    {
        // For Display
        Vector3 targetPosition = isOpen ? originalPosition : closedPosition;
        float arrowZ = isOpen ? -180f : 0f;
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

    // Fill with standard choices
    public void InitUpgradeCard()
    {
        int[] choices = new int[4] { -1, -2, -3,- 4};
        int excluded = Random.Range(-4, 0);
        for (int i = 0, j = 0; i < 4 && j < 3; i++)
        {
            if (choices[i] != excluded)
            {
                fillWithUpgrade(choiceArr[j], UpgradeConfigs.Instance._getUpgradeConfig(choices[i]));
                j++;
            }
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
        if (!isOpen)
        {
            expandBtn.onClick.Invoke();
        }
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
        upCard.IsWeapon = false;
        upCard.UpgradeData = upgradeConfig;
        upCard.fillUpgrade();
        // Change the color of the card based on the rarity level
        upCard.SetColor(upgradeConfig.rating);
    }

    // Fill with weapon
    private void fillWithWeapon(Button card, WeaponConfigs.WeaponConfig weaponConfig)
    {
        UpgradeCards upCard = card.GetComponent<UpgradeCards>();
        upCard.WeaponData = weaponConfig;
        upCard.fillWeapon();
        // Change the color of the card based on the rarity level
        upCard.SetColor(weaponConfig.rating);
    }

    // Choosing 
    public void choiceChosen(Button choice) {
        Players player = GameManager.Instance.GetLocalPlayer();
        if (player == null) {
            return;
        }
        UpgradeCards upCard = choice.GetComponent<UpgradeCards>();
        if (upCard.IsWeapon)
        {
            int weaponIndex = upCard.WeaponData.id;
            //read weaponConfig upCard.WeaponData
            if (!PhotonNetwork.IsConnected)
            {
                GameObject dropObj = Instantiate(PrefabManager.Instance.DroppedWeapon, player.transform.position, Quaternion.identity);
                DroppedItems dropped = dropObj.GetComponent<DroppedItems>();
                dropped.WeaponIndex = weaponIndex;
                dropped.SetUp();
            }
            else
            {
                GameObject dropObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.DroppedWeapon.name, player.transform.position, Quaternion.identity);
                DroppedItems dropped = dropObj.GetComponent<DroppedItems>();
                dropped.WeaponIndex = weaponIndex;
                dropped.SetUp();
            }
        }
        else {
            //read upgradeConfig upCard.UpgradeData
            UpgradeConfigs.UpgradeConfig upgradeConfig = upCard.UpgradeData;
            if (upgradeConfig.id <= -1) {
                player.SwapMesh(upgradeConfig.id);
            }
            player.AddEffect(upgradeConfig.specialEffectIndex, upgradeConfig.level);
            if (upgradeConfig.unique) {
                UpgradeConfigs.Instance.Locked.Add(upgradeConfig.id);
            }
        }
        pointsOff();
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
