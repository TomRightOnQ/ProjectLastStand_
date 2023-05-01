using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Bullets
[RequireComponent(typeof(PhotonView))]
public class Projectiles : Items, IPunObservable
{
    void Start()
    {
        gameObject.tag = "Proj";
    }

    // Projectile stats
    [SerializeField] private float damage = 1;
    [SerializeField] private int owner = 0;
    [SerializeField] private float life = 1.0f;
    [SerializeField] private bool selfDet = false;
    [SerializeField] private bool player = false;
    [SerializeField] private bool pen = false;
    [SerializeField] private bool aoe = false;
    [SerializeField] private float damageRange = 0.1f;
    [SerializeField] private bool _active = false;

    private bool currentState;
    private float creationTime;

    public const string UPDATE_PROJ = "UpdatePosition";

    // Sync
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      0. _active
        //      1. damage
        //      2. owner
        //      3. selfDet
        //      4. player
        //      5. pen
        //      6. aoe
        //      7. damageRange

        if (stream.IsWriting)
        {
            stream.SendNext(_active);
            stream.SendNext(damage);
            stream.SendNext(owner);
            stream.SendNext(life);
            stream.SendNext(selfDet);
            stream.SendNext(player);
            stream.SendNext(pen);
            stream.SendNext(aoe);
            stream.SendNext(damageRange);
        }
        else
        {
            _active = (bool)stream.ReceiveNext();
            damage = (float)stream.ReceiveNext();
            owner = (int)stream.ReceiveNext();
            life = (float)stream.ReceiveNext();
            selfDet = (bool)stream.ReceiveNext();
            player = (bool)stream.ReceiveNext();
            pen = (bool)stream.ReceiveNext();
            aoe = (bool)stream.ReceiveNext();
            damageRange = (float)stream.ReceiveNext();
            if (!_active && currentState) {
                Deactivate();
            } else if (_active && !currentState) {
                Activate();
            }
        }
    }

    public float Damage { get { return damage; } set { damage = value; } }
    public int Owner { get { return owner; } set { owner = value; } }
    public float Life { get { return life; } set { life = value; } }
    public bool SelfDet { get { return selfDet; } set { selfDet = value; } }
    public bool Player { get { return player; } set { player = value; } }
    public bool Pen { get { return pen; } set { pen = value; } }
    public bool AOE { get { return aoe; } set { aoe = value; } }

    public float DamageRange
    {
        get { return damageRange; }
        set { damageRange = value; }
    }

    // Control the lifespan of a projectile
    public override void OnEnable()
    {
        creationTime = Time.time;
        Invoke("Deactivate", life);
    }

    public override void OnDisable()
    {
        CancelInvoke("Deactivate");
    }

    public void Activate()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("_activate", RpcTarget.All);
        }
        else
        {
            _activate();
        }
    }

    public void Deactivate()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("_deactivate", RpcTarget.All);
        }
        else
        {
            _deactivate();
        }
    }

    [PunRPC]
    private void _activate()
    {
        gameObject.SetActive(true);
    }

    // Dying
    [PunRPC]
    private void _deactivate()
    {
        transform.position = new Vector3(-10f, -10f, -10f);
        _active = false;
        currentState = false;
        gameObject.SetActive(false);
        GameManager.Instance.dataManager.RemoveDeactivatedProj(this);
    }

    // Damage Detection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Monsters monster = other.gameObject.GetComponent<Monsters>();
            if (monster != null && monster.gameObject.activeSelf)
            {
                if (!AOE)
                {
                    gameObject.SetActive(false);
                    Debug.Log("Has taken damage");
                    Deactivate();
                    monster.TakeDamage(Damage);
                    GameManager.Instance.monsterManager.despawnCheck(monster);
                }
                else
                {
                    gameObject.SetActive(false);
                    // Push to AOE list
                    GameManager.Instance.DamageExplosions.Add(new DamageExplosion(transform.position, DamageRange, Damage));
                    Deactivate();
                }
            }
        }
    }
}
