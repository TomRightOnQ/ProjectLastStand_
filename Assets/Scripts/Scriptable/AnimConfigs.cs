using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Returning corresponding Animations
[CreateAssetMenu(menuName = "Configs/AnimConfigs")]
public class AnimConfigs : ScriptableSingleton<AnimConfigs>
{
    [SerializeField] private GameObject EXPAnim1;
    [SerializeField] private GameObject EXPAnim2;
    [SerializeField] private GameObject EXPAnim3;

    public GameObject GetAnim(int id) 
    {
        GameObject animation = null;

        switch (id)
        {
            case 0:
                animation = _getEXPAnim();
                break;
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
                Debug.LogWarning("Animation ID not found: " + id);
                break;
        }
        return animation;
    }
}
