using UnityEngine;

// Player a Sound
public class PlaySound : MonoBehaviour
{
    [SerializeField] private int id = 0;

    public void Play()
    {
        GameObject sfxObj = Instantiate(PrefabManager.Instance.SfxPrefab);
        SFXObjects sfx = sfxObj.GetComponent<SFXObjects>();
        sfx.IsOnetime = true;
        sfxObj.SetActive(true);
        sfx.SetUp(id);
    }
}
