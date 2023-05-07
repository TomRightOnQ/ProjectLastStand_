using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Base to defend!
public class Base : Entities, IPunObservable
{
    [SerializeField] GameObject gameOver;
    private static Base instance;

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
    }

    void Start()
    {
        gameObject.tag = "Base";
    }

    void Update()
    {
        UpdateHP();
        if (CurrentHitPoints <= 0) {
            gameOver.SetActive(true);
        }
    }

    // HP Bar 
    public void UpdateHP()
    {
        hpS.maxValue = hitPoints;
        hpS.value = currentHitPoints;
    }
}