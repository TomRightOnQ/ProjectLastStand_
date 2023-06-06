using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private GameObject monsterDummy;
    private const float ROT_SPD = 8f;
    private bool isWeapon = false;

    public void SetUp(int id, int count)
    {
        MonsterConfigs.MonsterConfig mosnterConfig = MonsterConfigs.Instance.getMonsterConfig(id);
        Mesh newMesh = ArtConfigs.Instance.getMesh(mosnterConfig.mesh);
        meshFilter.mesh = newMesh;
        number.text = " X " + count.ToString();
    }

    public void SetUpWeapon(int id, int level)
    {
        WeaponConfigs.WeaponConfig weaponConfig = WeaponConfigs.Instance._getWeaponConfig(id);
        Mesh newMesh = ArtConfigs.Instance.getMesh(weaponConfig.mesh);
        meshFilter.mesh = newMesh;
        number.text = "Level: " + (level + 1).ToString();
        monsterDummy.transform.localScale = new Vector3(2, 2, 2);
        isWeapon = true;
    }

    private void Update()
    {
        if (!isWeapon) {
            return;
        }
        monsterDummy.transform.Rotate(Vector3.up, ROT_SPD * Time.deltaTime);
    }
}
