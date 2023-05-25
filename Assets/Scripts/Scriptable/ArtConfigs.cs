using UnityEngine;

// Providing references for art assets
[CreateAssetMenu(menuName = "Configs/ArtConfigs")]
public class ArtConfigs : ScriptableSingleton<ArtConfigs>
{
    // Player
    [SerializeField] private Mesh HumanMesh;
    [SerializeField] private Mesh MageMesh;
    [SerializeField] private Mesh SkeletonMesh;
    [SerializeField] private Mesh SoldierMesh;

    // Weapon
    [SerializeField] private Mesh PistolMesh;
    [SerializeField] private Mesh RPGMesh;
    [SerializeField] private Mesh LMGMesh;
    [SerializeField] private Mesh HeatLaserMesh;
    [SerializeField] private Mesh LaserGunMesh;
    [SerializeField] private Mesh KornetMesh;

    // Projectiles
    [SerializeField] private Mesh DefaultProjMesh;
    [SerializeField] private Mesh DefaultMonsterProjMesh;
    [SerializeField] private Mesh KornetProjMesh;

    public enum Artconfig
    {
        // Player
        HumanMesh,
        MageMesh,
        SkeletonMesh,
        SoldierMesh,
        // Weapon
        PistolMesh,
        RPGMesh,
        LMGMesh,
        HeatLaserMesh,
        LaserGunMesh,
        KornetMesh,
        // Projectiles
        DefaultProj,
        DefaultMonsterProj,
        KornetProjMesh
    }

    public Mesh getMesh(string type)
    {
        Mesh ret = null;
        switch (type)
        {
            case "HumanMesh":
                ret = HumanMesh;
                break;
            case "MageMesh":
                ret = MageMesh;
                break;
            case "SkeletonMesh":
                ret = SkeletonMesh;
                break;
            case "SoldierMesh":
                ret = SoldierMesh;
                break;
        }
        return ret;
    }

    public Mesh getMesh(Artconfig type) 
    {
        Mesh ret = null;
        switch (type)
        {
        case Artconfig.HumanMesh:
            ret = HumanMesh;
            break;
        case Artconfig.MageMesh:
            ret = MageMesh;
            break;
        case Artconfig.SkeletonMesh:
            ret = SkeletonMesh;
            break;
        case Artconfig.SoldierMesh:
            ret = SoldierMesh;
            break;

        case Artconfig.PistolMesh:
            ret = PistolMesh;
            break;
        case Artconfig.RPGMesh:
            ret = RPGMesh;
            break;
        case Artconfig.LMGMesh:
            ret = LMGMesh;
            break;
        case Artconfig.HeatLaserMesh:
            ret = HeatLaserMesh;
            break;
        case Artconfig.LaserGunMesh:
            ret = LaserGunMesh;
            break;
        case Artconfig.KornetMesh:
            ret = KornetMesh;
            break;

        case Artconfig.DefaultProj:
            ret = DefaultProjMesh;
            break;
        case Artconfig.DefaultMonsterProj:
            ret = DefaultMonsterProjMesh;
            break;
        case Artconfig.KornetProjMesh:
            ret = KornetProjMesh;
            break;
        }
        return ret;
    }
}
