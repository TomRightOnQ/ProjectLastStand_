using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UpgradeConfigs;

// BuffIcons
public class BuffIcons : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;
    private UpgradeConfig upgradeData;

    // Set up an listing info
    public void SetUp(int id)
    {
        icon.sprite = ImageConfigs.Instance.GetEffectImage(id);
        text.text = UpgradeConfigs.Instance._getUpgradeConfig(id)._name;
    }
}
