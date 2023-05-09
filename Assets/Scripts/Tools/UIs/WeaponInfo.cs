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
    private bool isOpen = true;
    private float panelHeight;
    [SerializeField] private float speed = 10;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI attackText;

    // For weapon's info
    // Weapon Stats
    [SerializeField] private string wpName = "DeaultWeapon";
    [SerializeField] private int id = 0;
    [SerializeField] private int rating = 1;
    [SerializeField] private int type = 0;
    [SerializeField] private float attack = 10;
    [SerializeField] private float pen = 0.1f;
    [SerializeField] private float cd = 0.5f;
    [SerializeField] private int level = 1;

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
        panelHeight = panelTransform.rect.height;
        closedPosition = new Vector3(originalPosition.x, (panelHeight / 2), originalPosition.z);
    }

    void Update()
    {
        // For Display
        Vector3 targetPosition = isOpen ? originalPosition : closedPosition;
        panelTransform.anchoredPosition = Vector3.Lerp(panelTransform.anchoredPosition, targetPosition, speed * Time.deltaTime);
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
    }

    // Morph the weapon
    public void SetWeaponInfo(Weapons weapon)
    {
        id = weapon.ID;
        wpName = weapon.WpName;
        rating = weapon.Rating;
        type = weapon.Type;
        attack = weapon.Attack;
        attackText.text = "ATK: " + attack.ToString();
        nameText.text = wpName.ToString();
        levelText.text = "Lv." + level.ToString();
    }
}
