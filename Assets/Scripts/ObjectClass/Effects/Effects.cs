using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Effects and buffs
public class Effects : MonoBehaviourPunCallbacks
{
    [SerializeField] private Players player;
    private const string PREFAB_LOC = "Prefabs/";
    private void Awake()
    {
        player = GetComponent<Players>();
    }

    // SetUp
    public void SetUp(int index, int level)
    {
        switch (index) {
            case -1:
                Human(index);
                break;
            case -2:
                Mage(index);
                break;
            case -3:
                Skeleton(index);
                break;
            case -4:
                Soldier(index);
                break;
            case 0:
                Toughness(index, level);
                break;
            case 1:
                Swift(index, level);
                break;
            case 2:
                Damage(index, level);
                break;
            case 6:
                Magician(index, level);
                break;
            case 7:
                Precision(index, level);
                break;
            case 105:
                BloodSacrifice(index);
                break;
            case 203:
                SavageStriker(index);
                break;
            case 204:
                RocketMaster(index);
                break;
            case 205:
                Inferno(index);
                break;
            case 303:
                Rampage(index);
                break;
            case 304:
                RelentlessResent(index);
                break;
            case 305:
                TacticalShield(index);
                break;
            case 400:
                Immortal(index);
                break;
            case 401:
                Flight(index);
                break;
            case 402:
                DamageMAX(index);
                break;
            case 403:
                MeleeGrandMaster(index);
                break;
            case 404:
                GuardianAngel(index);
                break;
            case 405:
                Freezing(index);
                break;
            case 406:
                HolyNova(index);
                break;
            case 407:
                Assassination(index);
                break;
            default:
                break;
        }
    }

    public void Human(int id)
    {
        player.HitPoints += 4;
        player.CurrentHitPoints += 4;
        player.IsReady = true;
        player.UpdateStats();
    }

    public void Mage(int id)
    {
        player.WeaponDamageBase += 0.2f;
        player.IsReady = true;
        player.UpdateStats();
    }

    public void Skeleton(int id)
    {
        player.SpeedMod += 0.2f;
        player.WeaponDamageBase += 0.1f;
        player.DamageBase += 0.1f;
        player.DefaultDefence -= 0.1f;
        player.IsReady = true;
        player.UpdateStats();
    }

    public void Soldier(int id)
    {
        player.WeaponDamageBase += 0.2f;
        player.IsReady = true;
        player.UpdateStats();
    }

    // Functions for buffs
    public void Toughness(int id, int level)
    {
        float amount = (float)level + 1;
        player.HitPoints += amount;
        player.CurrentHitPoints += amount;
        player.AddToEffectList((level - 1) * 100 + id);
    }

    public void Swift(int id, int level)
    {
        float amount = (float)level * 0.02f + 0.04f;
        player.SpeedBase += amount;
        player.AddToEffectList((level - 1) * 100 + id);
    }

    public void Damage(int id, int level)
    {
        float amount = (float)level * 0.02f + 0.04f;
        player.DamageBase += amount;
        player.AddToEffectList((level - 1) * 100 + id);
    }

    public void Magician(int id, int level)
    {
        float amount = (float)level * 0.02f + 0.04f;
        player.WeaponDamageBase += amount;
        player.AddToEffectList((level - 1) * 100 + id);
    }

    public void Precision(int id, int level)
    {
        float amount = (float)level * 0.02f + 0.03f;
        player.CriticalRate += amount;
        player.AddToEffectList((level - 1) * 100 + id);
    }

    public void BloodSacrifice(int id) 
    {
        if (player.CheckCondition(id))
        {
            player.HitPoints -= 3;
            player.CurrentHitPoints -= 3;
            player.DamageBase += 0.05f;
            player.WeaponDamageBase += 0.07f;
            player.SpeedBase += 0.05f;
        } else {
            player.HitPoints -= 1;
            player.CurrentHitPoints -= 1;
            player.DamageBase += 0.01f;
            player.WeaponDamageBase += 0.02f;
            player.SpeedBase += 0.01f;
        }
        player.AddToEffectList(id);
    }

    public void SavageStriker(int id)
    {
        player.CriticalRate -= 0.1f;
        player.CriticalMod += 0.5f;
        player.AddToEffectList(id);
    }

    public void RocketMaster(int id)
    {
        player.EnhanceAoe();
        player.AddToEffectList(id);
    }

    public void Rampage(int id)
    {
        player.gameObject.AddComponent<Rampage>();
        player.AddToEffectList(id);
    }

    public void RampageEffect(int id)
    {
        player.gameObject.AddComponent<RampageEffect>();
        player.AddToEffectList(id);
    }

    public void RelentlessResent(int id)
    {
        player.gameObject.AddComponent<RelentlessResent>();
        player.AddToEffectList(id);
    }

    public void Immortal(int id)
    {
        player.HitPoints += 10;
        player.CurrentHitPoints += 10;
        player.gameObject.AddComponent<Immortal>();
        player.AddToEffectList(id);
    }

    public void Flight(int id)
    {
        player.SpeedBase += 0.3f;
        player.AddToEffectList(id);
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("RPCFlight", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void RPCFlight()
    {
        Players player = GameManager.Instance.GetLocalPlayer();
        player.SpeedBase += 0.1f;
    }

    public void DamageMAX(int id)
    {
        player.DamageMod = 1.2f;
        player.AddToEffectList(id);
    }
    public void MeleeGrandMaster(int id)
    {
        player.gameObject.AddComponent<MeleeGrandMaster>();
        player.AddToEffectList(id);
    }

    public void GuardianAngel(int id)
    {
        player.HitPoints *= 2;
        player.CurrentHitPoints = player.HitPoints;
        player.DefaultMagicDefence += 0.3f;
        player.TimeForRevive = 30f;
        player.AddToEffectList(id);
    }

    public void HolyNova(int id)
    {
        player.SetNova();
        player.AddToEffectList(id);
    }

    public void Assassination(int id)
    {
        player.gameObject.AddComponent<Assassination>();
        player.AddToEffectList(id);
    }

    public void Freezing(int id)
    {
        if (!PhotonNetwork.IsConnected)
        {
            GameObject ringObj = Instantiate(PrefabManager.Instance.IceRingPrefab);
            ringObj.transform.SetParent(transform);
            ringObj.transform.localPosition = Vector3.zero;
        }
        else {   
           GameObject ringObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.IceRingPrefab.name, Vector3.zero, Quaternion.identity);
            ringObj.transform.SetParent(transform);
            ringObj.transform.localPosition = Vector3.zero;
            PhotonView ringView = ringObj.GetComponent<PhotonView>();
            int ringViewID = ringView.ViewID;
            PhotonView playerView = GetComponent<PhotonView>();
            int playerViewID = playerView.ViewID;
            photonView.RPC("AddRingToPlayer", RpcTarget.AllBuffered, playerViewID, ringViewID);
        }
        player.AddToEffectList(id);
    }

    public void Inferno(int id)
    {
        if (!PhotonNetwork.IsConnected)
        {
            GameObject ringObj = Instantiate(PrefabManager.Instance.FireRingPrefab);
            ringObj.transform.SetParent(transform);
            ringObj.transform.localPosition = Vector3.zero;
        }
        else
        {
            GameObject ringObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.FireRingPrefab.name, Vector3.zero, Quaternion.identity);
            ringObj.transform.SetParent(transform);
            ringObj.transform.localPosition = Vector3.zero;
            PhotonView ringView = ringObj.GetComponent<PhotonView>();
            int ringViewID = ringView.ViewID;
            PhotonView playerView = GetComponent<PhotonView>();
            int playerViewID = playerView.ViewID;
            photonView.RPC("AddRingToPlayer", RpcTarget.AllBuffered, playerViewID, ringViewID);
        }
        player.AddToEffectList(id);
    }

    [PunRPC]
    public void AddRingToPlayer(int playerViewID, int ringViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        PhotonView ringView = PhotonView.Find(ringViewID);

        if (playerView != null && ringView != null)
        {
            Players player = playerView.GetComponent<Players>();
            Items ring = ringView.GetComponent<Items>();

            if (player != null && ring != null)
            {
                ring.transform.SetParent(player.transform);
                ring.transform.localPosition = Vector3.zero;
            }
        }
    }

    public void TacticalShield(int id)
    {
        if (!PhotonNetwork.IsConnected)
        {
            GameObject ringObj = Instantiate(PrefabManager.Instance.TacticalShieldRingPrefab);
            ringObj.transform.SetParent(transform);
            ringObj.transform.localPosition = Vector3.zero;
        }
        else
        {
            GameObject ringObj = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.TacticalShieldRingPrefab.name, Vector3.zero, Quaternion.identity);
            ringObj.transform.SetParent(transform);
            ringObj.transform.localPosition = Vector3.zero;
            PhotonView ringView = ringObj.GetComponent<PhotonView>();
            int ringViewID = ringView.ViewID;
            PhotonView playerView = GetComponent<PhotonView>();
            int playerViewID = playerView.ViewID;
            photonView.RPC("AddRingToPlayer", RpcTarget.AllBuffered, playerViewID, ringViewID);
        }
        player.AddToEffectList(id);
    }
}


