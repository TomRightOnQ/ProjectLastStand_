using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterConfigs;

// Boss 2
public class Hyperion : Monsters
{
    [SerializeField] private MeshRenderer _renderer;

    private void Awake()
    {
        IsBoss = true;
        MonsterConfigs.MonsterConfig config = MonsterConfigs.Instance.getMonsterConfig(6);
        prefabManager = Resources.Load<PrefabManager>("PrefabManager");
        SetHyperion(config);
    }

    private void SetHyperion(MonsterConfig MonsterConfigs)
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

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // Taking Damage
    public override void TakeDamage(float damage, bool isMagic)
    {
        base.TakeDamage(damage, isMagic);
    }

    public override void SwapMaterial(int id)
    {
        Material material = ArtConfigs.Instance.getMaterial(id);
        if (material != null)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("RPCSwapMaterial", RpcTarget.All, id);
            }
            else
            {
                _renderer.material = material;
            }
        }
    }

    public override void PlayHitAnim(Vector3 pos)
    {
        if (AnimConfigs.Instance.GetAnim(0) == null)
            return;
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(0), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    [PunRPC]
    public override void RPCPlayHitAnim(int id, Vector3 pos, float scale)
    {
        if (AnimConfigs.Instance.GetAnim(id) == null)
            return;
        GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(id), Vector3.zero, Quaternion.identity);
        animObject.transform.position = pos;
        animObject.transform.localRotation = Quaternion.Euler(45, 0, 0);
        animObject.transform.localScale = new Vector3(scale, scale, scale);
    }
}
