using UnityEngine;

public class TriggerWall : MonoBehaviour
{
    [SerializeField] private Monsters monster;
    [SerializeField] private GameObject wall;

    private void OnDisable()
    {
        if (monster.CurrentHitPoints <= 0.1) {
            wall.SetActive(false);
        }
    }
}
