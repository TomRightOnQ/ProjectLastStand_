using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boss 2
public class Hyperion : Monsters
{
    private void Awake()
    {
        IsBoss = true;
        MonsterConfigs.MonsterConfig config = MonsterConfigs.Instance.getMonsterConfig(6);
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
