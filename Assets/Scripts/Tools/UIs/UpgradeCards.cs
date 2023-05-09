using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UpgradeConfigs;
using static WeaponConfigs;

// Define single upgrade cards
public class UpgradeCards : MonoBehaviour
{
    // flag the return type
    private bool isWeapon = false;
    private WeaponConfig weaponData;
    private UpgradeConfig upgradeData;

    public bool IsWeapon { get { return isWeapon; } set { isWeapon = value; } }
    public WeaponConfig WeaponData { get { return weaponData; } set { weaponData = value; } }
    public UpgradeConfig UpgradeData { get { return upgradeData; } set { upgradeData = value; } }

    public void Start()
    {
        Image cardImage = GetComponent<Image>();
        cardImage.color = new Color(0.5f, 0f, 1f);
    }

    public void fillUpgrade()
    {
        isWeapon = false;
        // Get the text components of the card
        TextMeshProUGUI nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI infoText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        nameText.text = UpgradeData._name;
        infoText.text = UpgradeData.description;
    }
    public void fillWeapon()
    {
        isWeapon = true;
        // Get the text components of the card
        TextMeshProUGUI nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI infoText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        // Fill the name and info text
        nameText.text = weaponData._name;
        infoText.text = weaponData.intro + "\n\n" + "<i>" + weaponData.info + "</i>";
    }
}
