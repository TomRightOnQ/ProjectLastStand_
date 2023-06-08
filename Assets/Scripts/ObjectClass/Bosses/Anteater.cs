using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterConfigs;

// Boss 1
public class Anteater : Monsters
{
    private void Awake()
    {
        IsBoss = true;
        MonsterConfigs.MonsterConfig config = MonsterConfigs.Instance.getMonsterConfig(7);
        SetAnteater(config);
    }

    private void SetAnteater(MonsterConfig MonsterConfigs)
    {
        name = MonsterConfigs._name + "...?";
        hitPoints = MonsterConfigs.hitPoints;
        currentHitPoints = hitPoints;
        speed = MonsterConfigs.speed;
        exp = MonsterConfigs.exp;
        defaultAttack = MonsterConfigs.defaultAttack;
        defaultWeaponAttack = MonsterConfigs.defaultWeaponAttack;
        defaultDefence = MonsterConfigs.defaultDefence;
        defaultMagicDefence = MonsterConfigs.defaultMagicDefence;
        prevHP = currentHitPoints;
        behaviorType = MonsterConfigs.behaviorType;
        monsterAI.SetUp();
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
