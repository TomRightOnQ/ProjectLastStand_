using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UpgradeConfigs;
using static WeaponConfigs;

// Define single upgrade cards
public class UpgradeCards : MonoBehaviour
{
    // flag the return type
    [SerializeField] Image cardImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] private bool isWeapon = false;
    private WeaponConfig weaponData;
    private UpgradeConfig upgradeData;

    public bool IsWeapon { get { return isWeapon; } set { isWeapon = value; } }
    public WeaponConfig WeaponData { get { return weaponData; } set { weaponData = value; } }
    public UpgradeConfig UpgradeData { get { return upgradeData; } set { upgradeData = value; } }

    public void Start()
    {
        cardImage.color = new Color(0.5f, 0f, 1f);
    }

    public void fillUpgrade()
    {
        isWeapon = false;
        nameText.text = UpgradeData._name;
        infoText.text = UpgradeData.description;
    }
    public void fillWeapon()
    {
        isWeapon = true;
        // Fill the name and info text
        nameText.text = weaponData._name;
        infoText.text = weaponData.intro + "\n\n" + "<i>" + weaponData.info + "</i>";
    }

    public void SetColor(int rating)
    {
        switch (rating)
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
