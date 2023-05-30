using UnityEngine;
using Photon.Pun;

// Base to defend!
public class Base : Entities, IPunObservable
{
    [SerializeField] GameObject gameOver;

    private static Base instance;
    private bool dead = false;

    public static Base Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Base>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Check repeated instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        gameObject.tag = "Base";
    }

    protected override void Update()
    {
        if (dead) {
            return;
        }
        currentHitPoints += 0.01f;
        UpdateHP();
        if (CurrentHitPoints <= 0) {
            GameManager.Instance.GameOver();
            dead = true;
        }
    }

    // HP Bar 
    public void UpdateHP()
    {
        hpS.maxValue = hitPoints;
        hpS.value = currentHitPoints;
        if (currentHitPoints >= hitPoints) {
            currentHitPoints = hitPoints;
        }
    }
}
