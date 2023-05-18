using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Providing references for art assets
[CreateAssetMenu(menuName = "Configs/ArtConfigs")]
public class ArtConfigs : ScriptableSingleton<ArtConfigs>
{
    [SerializeField] private Mesh HumanMesh;
    [SerializeField] private Mesh MageMesh;
    [SerializeField] private Mesh SkeletonMesh;
    [SerializeField] private Mesh SoldierMesh;

    [SerializeField] private Mesh PistolMesh;
    [SerializeField] private Mesh RPGMesh;
    [SerializeField] private Mesh LMGMesh;
    [SerializeField] private Mesh HeatLaserMesh;
    [SerializeField] private Mesh LaserGunMesh;

    public enum Artconfig
    {
        HumanMesh,
        MageMesh,
        SkeletonMesh,
        SoldierMesh,
        PistolMesh,
        RPGMesh,
        LMGMesh,
        HeatLaserMesh,
        LaserGunMesh
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
        }
        return ret;
    }
}
