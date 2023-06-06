using UnityEngine;

// Returning corresponding Animations
[CreateAssetMenu(menuName = "Configs/AnimConfigs")]
public class AnimConfigs : ScriptableSingleton<AnimConfigs>
{
    [SerializeField] private GameObject EXPAnim1;
    [SerializeField] private GameObject EXPAnim2;
    [SerializeField] private GameObject EXPAnim3;

    [SerializeField] private GameObject BladeAnim;

    public GameObject GetAnim(int id) 
    {
        GameObject animation;
        switch (id)
        {
            case 0:
                animation = _getEXPAnim();
                break;
            case 1:
                animation = BladeAnim;
                break;
            default:
                return null;
        }
        return animation;
    }
    public GameObject _getEXPAnim()
    {
        GameObject animation = null;
        int id = Random.Range(0, 3);;
        switch (id)
        {
            case 0:
                animation = EXPAnim1;
                break;
            case 1:
                animation = EXPAnim2;
                break;
            case 2:
                animation = EXPAnim3;
                break;
            default:
                break;
        }
        return animation;
    }
}
