using UnityEngine;
using UnityEngine.UI;
using static UpgradeConfigs;

// BuffIcons
public class BuffIcons : MonoBehaviour
{
    [SerializeField] private Image icon;
    private UpgradeConfig upgradeData;

    // Set up an listing info
    public void SetUp(int id)
    {
        icon.sprite = ImageConfigs.Instance.GetEffectImage(id);
    }
}
