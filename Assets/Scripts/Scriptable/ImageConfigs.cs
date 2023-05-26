using UnityEngine;
using UnityEngine.UI;

// Sprite references
[CreateAssetMenu(menuName = "Configs/ImageConfigs")]
public class ImageConfigs : ScriptableSingleton<ImageConfigs>
{
    [SerializeField] private Sprite toughnessIcon;
    [SerializeField] private Sprite swiftIcon;
    [SerializeField] private Sprite damageIcon;
    [SerializeField] private Sprite defenderIcon;
    [SerializeField] private Sprite counterIcon;
    [SerializeField] private Sprite magicianIcon;
    [SerializeField] private Sprite precisionIcon;
    [SerializeField] private Sprite bloodSacrificeIcon;
    [SerializeField] private Sprite savageStrikerIcon;
    [SerializeField] private Sprite rocketMasterIcon;
    [SerializeField] private Sprite rampageIcon;
    [SerializeField] private Sprite relentlessResentIcon;
    [SerializeField] private Sprite immortalIcon;
    [SerializeField] private Sprite flightIcon;
    [SerializeField] private Sprite damageMaxIcon;
    [SerializeField] private Sprite meleeGrandMasterIcon;
    [SerializeField] private Sprite guardianAngelIcon;
    [SerializeField] private Sprite holyNovaIcon;
    [SerializeField] private Sprite assassinationIcon;

    [SerializeField] private Sprite DefaultIcon;
    public Sprite ToughnessIcon { get { return toughnessIcon; } }
    public Sprite SwiftIcon { get { return swiftIcon; } }
    public Sprite DamageIcon { get { return damageIcon; } }
    public Sprite DefenderIcon { get { return defenderIcon; } }
    public Sprite CounterIcon { get { return counterIcon; } }
    public Sprite MagicianIcon { get { return magicianIcon; } }
    public Sprite PrecisionIcon { get { return precisionIcon; } }
    public Sprite BloodSacrificeIcon { get { return bloodSacrificeIcon; } }
    public Sprite SavageStrikerIcon { get { return savageStrikerIcon; } }
    public Sprite RocketMasterIcon { get { return rocketMasterIcon; } }
    public Sprite RampageIcon { get { return rampageIcon; } }
    public Sprite RelentlessResentIcon { get { return relentlessResentIcon; } }
    public Sprite ImmortalIcon { get { return immortalIcon; } }
    public Sprite FlightIcon { get { return flightIcon; } }
    public Sprite DamageMaxIcon { get { return damageMaxIcon; } }
    public Sprite MeleeGrandMasterIcon { get { return meleeGrandMasterIcon; } }
    public Sprite GuardianAngelIcon { get { return guardianAngelIcon; } }
    public Sprite HolyNovaIcon { get { return holyNovaIcon; } }
    public Sprite AssassinationIcon { get { return assassinationIcon; } }

    public Sprite GetEffectImage(int id)
    {
        Debug.Log("Getting pic " + id);
        switch (id)
        {
            case 0:
                return ToughnessIcon;
            case 1:
                return SwiftIcon;
            case 2:
                return DamageIcon;
            case 3:
                return DefenderIcon;
            case 4:
                return CounterIcon;
            case 6:
                return MagicianIcon;
            case 7:
                return PrecisionIcon;
            case 105:
                return BloodSacrificeIcon;
            case 203:
                return SavageStrikerIcon;
            case 204:
                return RocketMasterIcon;
            case 303:
                return RampageIcon;
            case 304:
                return RelentlessResentIcon;
            case 400:
                return ImmortalIcon;
            case 401:
                return FlightIcon;
            case 402:
                return DamageMaxIcon;
            case 403:
                return MeleeGrandMasterIcon;
            case 404:
                return GuardianAngelIcon;
            case 406:
                return HolyNovaIcon;
            case 407:
                return AssassinationIcon;
            default:
                return DefaultIcon;
        }
    }
}
