using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static WeaponConfigs;

// Weapon Info cards on screen bottom-left corner
public class WeaponInfo : MonoBehaviour
{
    // For Display
    [SerializeField] private RectTransform panelTransform;
    private Vector3 originalPosition;
    private Vector3 closedPosition;
    [SerializeField] private bool isOpen = true;
    private float panelHeight;
    [SerializeField] private float speed = 10;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI slotText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private bool movable = true;
    [SerializeField] private bool choice = false; // If used as a weapon choice
    [SerializeField] private WeaponInfo theOtherChoice;
    [SerializeField] private TextMeshProUGUI weaponHintText;

    // For weapon's info
    // Weapon Stats
    [SerializeField] private string wpName = "DeaultWeapon";
    private int id = 0;
    private int rating = 1;
    private int type = 0;
    private float attack = 10;
    private float pen = 0.1f;
    private float cd = 0.5f;
    private int level = 1;

    [SerializeField] private float verticalDistance = -1;

    // Class properties
    public string WpName { get { return wpName; } set { wpName = value; } }
    public int ID { get { return id; } set { id = value; } }
    public int Type { get { return type; } set { type = value; } }
    public float Attack { get { return attack; } set { attack = value; } }
    public float Pen { get { return pen; } set { pen = value; } }
    public float CD { get { return cd; } set { cd = value; } }

    void Start()
    {
        // For Display
        originalPosition = panelTransform.anchoredPosition;
        if (verticalDistance == -1)
        {
            panelHeight = panelTransform.rect.height;
        }
        else {
            panelHeight = verticalDistance;
        }

        closedPosition = new Vector3(originalPosition.x, (panelHeight / 2), originalPosition.z);
    }

    void Update()
    {
        // For Display
        Vector3 targetPosition = isOpen ? originalPosition : closedPosition;
        panelTransform.anchoredPosition = Vector3.Lerp(panelTransform.anchoredPosition, targetPosition, speed * Time.deltaTime);
        if (choice) {
            weaponHintText.gameObject.SetActive(!isOpen);
        }
    }

    public void ToggleMenu()
    {
        if (!movable) return;
        isOpen = !isOpen;
        if (choice && !theOtherChoice.isOpen) {
            theOtherChoice.CloseMenu();
        }
    }

    public void CloseMenu()
    {
        isOpen = true;
    }

    // Morph the weapon
    public void SetWeaponInfo(int slot, Weapons weapon)
    {
        id = weapon.ID;
        wpName = weapon.WpName;
        rating = weapon.Rating;
        type = weapon.Type;
        attack = weapon.Atk;
        level = weapon.Level;
        attackText.text = "ATK: " + attack.ToString();
        nameText.text = wpName.ToString();
        levelText.text = "Lv." + level.ToString();
        slotText.text = (slot + 1).ToString();
    }

    // Morph the weapon
    public void SetWeaponInfo(int slot, WeaponConfig weapon, int _level)
    {
        id = weapon.id;
        wpName = weapon._name;
        rating = weapon.rating;
        type = weapon.type;
        attack = weapon.attack;
        attackText.text = "ATK: " + attack.ToString();
        nameText.text = wpName.ToString();
        level = _level;
        levelText.text = "Lv." + level.ToString();
        slotText.text = (slot + 1).ToString();
    }
}
