using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boss 1
public class Anteater : Monsters
{
    private void Awake()
    {
        IsBoss = true;
        MonsterConfigs.MonsterConfig config = MonsterConfigs.Instance.getMonsterConfig(7);
        SetMonsters(config);
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
    }

    // Update is called once per frame
    protected override void Update()
    {

    }
}
