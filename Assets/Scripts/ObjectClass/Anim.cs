using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animations
public class Anim : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private int id;
    [SerializeField] private float life = 1;

    public string Name { get { return _name; } }
    public int Id { get { return id; } }
    public float Life { get { return life; } }

    private void Start()
    {
        Destroy(gameObject, life);
    }
}
