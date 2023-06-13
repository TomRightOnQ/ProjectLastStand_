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
    [SerializeField] private Sprite iceRingIcon;
    [SerializeField] private Sprite fireRingIcon;
    [SerializeField] private Sprite tacticalShieldIcon;
    [SerializeField] private Sprite defenderII;
    [SerializeField] private Sprite counterII;
    [SerializeField] private Sprite damageII;
    [SerializeField] private Sprite damageIII;
    [SerializeField] private Sprite damageIV;
    [SerializeField] private Sprite magicianII;
    [SerializeField] private Sprite magicianIII;
    [SerializeField] private Sprite magicianIV;
    [SerializeField] private Sprite speedII;
    [SerializeField] private Sprite speedIII;
    [SerializeField] private Sprite speedIV;
    [SerializeField] private Sprite toughnessII;
    [SerializeField] private Sprite toughnessIII;
    [SerializeField] private Sprite toughnessIV;
    [SerializeField] private Sprite swiftII;
    [SerializeField] private Sprite swiftIII;
    [SerializeField] private Sprite swiftIV;
    [SerializeField] private Sprite preciscionII;
    [SerializeField] private Sprite preciscionIII;
    [SerializeField] private Sprite preciscionIV;
    [SerializeField] private Sprite defaultIcon;

    [SerializeField] private Sprite pistolThumb;
    [SerializeField] private Sprite daggerThumb;
    [SerializeField] private Sprite lmgThumb;
    [SerializeField] private Sprite heatThumb;
    [SerializeField] private Sprite laserThumb;
    [SerializeField] private Sprite bloodSwordThumb;
    [SerializeField] private Sprite kerisThumb;
    [SerializeField] private Sprite kornetThumb;
    [SerializeField] private Sprite forkThumb;
    [SerializeField] private Sprite rpgThumb;
    [SerializeField] private Sprite wandThumb;
    public Sprite ToughnessIcon => toughnessIcon;
    public Sprite SwiftIcon => swiftIcon;
    public Sprite DamageIcon => damageIcon;
    public Sprite DefenderIcon => defenderIcon;
    public Sprite CounterIcon => counterIcon;
    public Sprite MagicianIcon => magicianIcon;
    public Sprite PrecisionIcon => precisionIcon;
    public Sprite BloodSacrificeIcon => bloodSacrificeIcon;
    public Sprite SavageStrikerIcon => savageStrikerIcon;
    public Sprite RocketMasterIcon => rocketMasterIcon;
    public Sprite RampageIcon => rampageIcon;
    public Sprite RelentlessResentIcon => relentlessResentIcon;
    public Sprite ImmortalIcon => immortalIcon;
    public Sprite FlightIcon => flightIcon;
    public Sprite DamageMaxIcon => damageMaxIcon;
    public Sprite MeleeGrandMasterIcon => meleeGrandMasterIcon;
    public Sprite GuardianAngelIcon => guardianAngelIcon;
    public Sprite HolyNovaIcon => holyNovaIcon;
    public Sprite AssassinationIcon => assassinationIcon;
    public Sprite IceRingIcon => iceRingIcon;
    public Sprite FireRingIcon => fireRingIcon;
    public Sprite TacticalShieldIcon => tacticalShieldIcon;
    public Sprite DefenderII => defenderII;
    public Sprite CounterII => counterII;
    public Sprite DamageII => damageII;
    public Sprite DamageIII => damageIII;
    public Sprite DamageIV => damageIV;
    public Sprite MagicianII => magicianII;
    public Sprite MagicianIII => magicianIII;
    public Sprite MagicianIV => magicianIV;
    public Sprite SpeedII => speedII;
    public Sprite SpeedIII => speedIII;
    public Sprite SpeedIV => speedIV;
    public Sprite DefaultIcon => defaultIcon;
    public Sprite ToughnessII => toughnessII;
    public Sprite ToughnessIII => toughnessIII;
    public Sprite ToughnessIV => toughnessIV;
    public Sprite SwiftII => swiftII;
    public Sprite SwiftIII => swiftIII;
    public Sprite SwiftIV => swiftIV;
    public Sprite PrecisionII => preciscionII;
    public Sprite PrecisionIII => preciscionIII;
    public Sprite PrecisionIV => preciscionIV;

    public Sprite PistolThumb => pistolThumb;
    public Sprite DaggerThumb => daggerThumb;
    public Sprite LMGThumb => lmgThumb;
    public Sprite HeatThumb => heatThumb;
    public Sprite LaserThumb => laserThumb;
    public Sprite BloodSwordThumb => bloodSwordThumb;
    public Sprite KerisThumb => kerisThumb;
    public Sprite KornetThumb => kornetThumb;
    public Sprite ForkThumb => forkThumb;
    public Sprite RPGThumb => rpgThumb;
    public Sprite WandThumb => wandThumb;

    public Sprite GetEffectImage(int id)
    {
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
            case 100:
                return ToughnessII;
            case 101:
                return SwiftII;
            case 102:
                return DamageII;
            case 103:
                return DefenderII;
            case 104:
                return CounterII;
            case 105:
                return BloodSacrificeIcon;
            case 106:
                return MagicianII;
            case 107:
                return PrecisionII;
            case 200:
                return ToughnessIII;
            case 201:
                return SwiftIII;
            case 202:
                return DamageIII;
            case 203:
                return SavageStrikerIcon;
            case 204:
                return RocketMasterIcon;
            case 205:
                return FireRingIcon;
            case 206:
                return MagicianIII;
            case 207:
                return PrecisionIII;
            case 300:
                return ToughnessIV;
            case 301:
                return SwiftIV;
            case 302:
                return DamageIV;
            case 303:
                return RampageIcon;
            case 304:
                return RelentlessResentIcon;
            case 305:
                return TacticalShieldIcon;
            case 306:
                return MagicianIV;
            case 307:
                return PrecisionIV;
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
            case 405:
                return IceRingIcon;
            case 406:
                return HolyNovaIcon;
            case 407:
                return AssassinationIcon;
            default:
                return DefaultIcon;
        }
    }

    public Sprite GetWeaponImage(int id)
    {
        switch (id)
        {
            case 0:
                return PistolThumb;
            case 1:
                return LMGThumb;
            case 2:
                return WandThumb;
            case 301:
                return DaggerThumb;
            case 100:
                return RPGThumb;
            case 101:
                return HeatThumb;
            case 102:
                return BloodSwordThumb;
            case 200:
                return LaserThumb;
            case 201:
                return KerisThumb;
            case 300:
                return KornetThumb;
            case -2:
                return ForkThumb;
            default:
                return PistolThumb;
        }
    }
}
