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
    // Monster
    [SerializeField] private Mesh DefaultMonsterMesh;
    [SerializeField] private Mesh LightingMonsterMesh;
    [SerializeField] private Mesh ZealotMonsterMesh;
    [SerializeField] private Mesh SwarmMonsterMesh;
    [SerializeField] private Mesh SlapMonsterMesh;
    [SerializeField] private Mesh MageKillerMonsterMesh;
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

    // Materials
    [SerializeField] private Material DefaultMaterial;
    [SerializeField] private Material DefaultProjMaterial;
    [SerializeField] private Material ProjMaterial;

    public enum Artconfig
    {
        // Player
        HumanMesh,
        MageMesh,
        SkeletonMesh,
        SoldierMesh,
        // Momster
        DefaultMonsterMesh,
        LightingMonsterMesh,
        ZealotMonsterMesh,
        SwarmMonsterMesh,
        SlapMonsterMesh,
        MageKillerMonsterMesh,
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
    public Material getMaterial(int id)
    {
        Material ret = null;
        switch (id)
        {
            case 0:
                ret = DefaultMaterial;
                break;
            case 1:
                ret = DefaultProjMaterial;
                break;
        }
        return ret;
    }

    public Material getMaterial(Artconfig type)
    {
        Material ret = null;
        switch (type)
        {
            case Artconfig.HumanMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.MageMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.SkeletonMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.SoldierMesh:
                ret = DefaultMaterial;
                break;

            case Artconfig.PistolMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.RPGMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.LMGMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.HeatLaserMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.LaserGunMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.KornetMesh:
                ret = DefaultMaterial;
                break;

            case Artconfig.DefaultMonsterMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.LightingMonsterMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.ZealotMonsterMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.SwarmMonsterMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.SlapMonsterMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.MageKillerMonsterMesh:
                ret = DefaultMaterial;
                break;

            case Artconfig.DefaultProj:
                ret = DefaultProjMaterial;
                break;
            case Artconfig.DefaultMonsterProj:
                ret = DefaultProjMaterial;
                break;
            case Artconfig.KornetProjMesh:
                ret = ProjMaterial;
                break;
        }
        return ret;
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
            // Player Meshes
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

            // Weapon Meshes
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

            // Projectile Meshes
            case Artconfig.DefaultProj:
                ret = DefaultProjMesh;
                break;
            case Artconfig.DefaultMonsterProj:
                ret = DefaultMonsterProjMesh;
                break;
            case Artconfig.KornetProjMesh:
                ret = KornetProjMesh;
                break;

            // Monster Meshes
            case Artconfig.DefaultMonsterMesh:
                ret = DefaultMonsterMesh;
                break;
            case Artconfig.LightingMonsterMesh:
                ret = LightingMonsterMesh;
                break;
            case Artconfig.ZealotMonsterMesh:
                ret = ZealotMonsterMesh;
                break;
            case Artconfig.SwarmMonsterMesh:
                ret = SwarmMonsterMesh;
                break;
            case Artconfig.SlapMonsterMesh:
                ret = SlapMonsterMesh;
                break;
            case Artconfig.MageKillerMonsterMesh:
                ret = MageKillerMonsterMesh;
                break;
        }
        return ret;
    }
}
