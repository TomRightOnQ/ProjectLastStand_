using UnityEngine;
using UnityEngine.Audio;

// SFX Objects
public class SFXObjects : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private int id = 0;
    [SerializeField] private float life = 1f;
    [SerializeField] private bool isOnetime = false;

    public bool IsOnetime { set { isOnetime = value; } }

    public void SetUp(int _id)
    {
        id = _id;
        source.clip = AudioConfigs.Instance.GetAudio(id);
        if (source.clip == null) {
            Deactivate();
        }
        life = source.clip.length;
        source.Play();
        Invoke("Deactivate", life);
    }

    public void Activate()
    {
        _activate();
    }

    public void Deactivate()
    {
        CancelInvoke("Deactivate");
        _deactivate();
    }

    private void _activate()
    {
        gameObject.SetActive(true);
    }

    private void _deactivate()
    {
        source.Stop();
        gameObject.SetActive(false);
        if (isOnetime) {
            Destroy(this.gameObject);
        }
    }
}
