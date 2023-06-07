using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterConfigs;

// Boss #3
public class Leviathan : Monsters
{
    [SerializeField] private MeshCollider wCollider;

    private void Awake()
    {
        IsBoss = true;
        MonsterConfig config = MonsterConfigs.Instance._getMonsterConfig();
        SetMonsters(config);
        SetLe(config);
    }

    private void SetLe(MonsterConfig MonsterConfigs)
    {
        name = MonsterConfigs._name + "...?";
        hitPoints = MonsterConfigs.hitPoints * 25f;
        currentHitPoints = hitPoints;
        speed = MonsterConfigs.speed * 0.25f;
        exp = 26;
        defaultAttack = MonsterConfigs.defaultAttack * 1.5f;
        defaultWeaponAttack = MonsterConfigs.defaultWeaponAttack * 1.5f;
        defaultDefence = MonsterConfigs.defaultDefence;
        defaultMagicDefence = MonsterConfigs.defaultMagicDefence;
        prevHP = currentHitPoints;
        behaviorType = MonsterBehaviorType.Walker;
        monsterAI.SetUp();
        SwapMonsterMesh(MonsterConfigs.id);
    }

    public void SwapMonsterCollider(int id)
    {
        MonsterConfig monsterConfig = MonsterConfigs.Instance.getMonsterConfig(id);
        Mesh mesh = ArtConfigs.Instance.getMesh(monsterConfig.mesh);
        // Call the RPC to synchronize the mesh change across the network
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPCSwapMonsterCollider", RpcTarget.All, id);
        }
        else
        {
            wCollider.sharedMesh = mesh;
        }
    }

    [PunRPC]
    public void RPCSwapMonsterCollider(int id)
    {
        MonsterConfig monsterConfig = MonsterConfigs.Instance.getMonsterConfig(id);
        Mesh mesh = ArtConfigs.Instance.getMesh(monsterConfig.mesh);
        wCollider.sharedMesh = mesh;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            Base _base = other.gameObject.GetComponent<Base>();
            if (_base != null)
            {
                _base.TakeDamage(defaultAttack * defaultWeaponAttack * 12.5f, false);
            }
            GameManager.Instance.monsterManager.despawnForce(this);
        }
        else if (other.CompareTag("Player"))
        {
            Players _player = other.gameObject.GetComponent<Players>();
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z - 1.5f);
            PlayHitAnim(pos);
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCPlayHitAnim", RpcTarget.Others, 0, pos, 1f);
            }
            if (_player != null)
            {
                _player.TakeDamage(defaultAttack * defaultWeaponAttack / 2, false);
            }
        }
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
