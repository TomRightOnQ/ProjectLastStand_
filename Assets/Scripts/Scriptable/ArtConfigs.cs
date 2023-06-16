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
    [SerializeField] private Mesh ForkMesh;
    [SerializeField] private Mesh EvilDaggerMesh;
    [SerializeField] private Mesh BloodSwordMesh;
    [SerializeField] private Mesh KerisMesh;
    [SerializeField] private Mesh PistolMesh;
    [SerializeField] private Mesh RPGMesh;
    [SerializeField] private Mesh LMGMesh;
    [SerializeField] private Mesh HeatLaserMesh;
    [SerializeField] private Mesh LaserGunMesh;
    [SerializeField] private Mesh KornetMesh;
    [SerializeField] private Mesh WandMesh;
    // Projectiles
    [SerializeField] private Mesh DefaultProjMesh;
    [SerializeField] private Mesh DefaultMonsterProjMesh;
    [SerializeField] private Mesh KornetProjMesh;
    [SerializeField] private Mesh WandProj;
    // Materials
    [SerializeField] private Material DefaultMaterial;
    [SerializeField] private Material DefaultProjMaterial;
    [SerializeField] private Material EnemyProjMaterial;
    [SerializeField] private Material ProjMaterial;
    [SerializeField] private Material MagicMaterial;

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
        ForkMesh,
        EvilDaggerMesh,
        BloodSwordMesh,
        KerisMesh,
        PistolMesh,
        RPGMesh,
        LMGMesh,
        HeatLaserMesh,
        LaserGunMesh,
        KornetMesh,
        WandMesh,
        // Projectiles
        DefaultProj,
        DefaultMonsterProj,
        KornetProjMesh,
        WandProj,
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
            case 2:
                ret = EnemyProjMaterial;
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
            case Artconfig.MageMesh:
            case Artconfig.SkeletonMesh:
            case Artconfig.SoldierMesh:
            case Artconfig.PistolMesh:
            case Artconfig.RPGMesh:
            case Artconfig.LMGMesh:
            case Artconfig.HeatLaserMesh:
            case Artconfig.LaserGunMesh:
            case Artconfig.KornetMesh:
            case Artconfig.DefaultMonsterMesh:
            case Artconfig.LightingMonsterMesh:
            case Artconfig.ZealotMonsterMesh:
            case Artconfig.SwarmMonsterMesh:
            case Artconfig.SlapMonsterMesh:
            case Artconfig.MageKillerMonsterMesh:
            case Artconfig.WandMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.DefaultProj:
            case Artconfig.KornetProjMesh:
                ret = DefaultProjMaterial;
                break;
            case Artconfig.DefaultMonsterProj:
                ret = EnemyProjMaterial;
                break;
            case Artconfig.ForkMesh:
            case Artconfig.EvilDaggerMesh:
            case Artconfig.BloodSwordMesh:
            case Artconfig.KerisMesh:
                ret = DefaultMaterial;
                break;
            case Artconfig.WandProj:
                ret = MagicMaterial;
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
            case Artconfig.ForkMesh:
                ret = ForkMesh;
                break;
            case Artconfig.EvilDaggerMesh:
                ret = EvilDaggerMesh;
                break;
            case Artconfig.BloodSwordMesh:
                ret = BloodSwordMesh;
                break;
            case Artconfig.KerisMesh:
                ret = KerisMesh;
                break;
            case Artconfig.WandMesh:
                ret = WandMesh;
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
            case Artconfig.WandProj:
                ret = WandProj;
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
