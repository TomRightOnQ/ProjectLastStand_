using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

// Entities are all NPCs and player-controlled units
public abstract class Entities : DefaultObjects, IPunObservable
{

    // Player Stats
    [SerializeField] protected float hitPoints = 20;
    [SerializeField] protected float currentHitPoints = 20;
    [SerializeField] protected float defaultAttack = 1;
    [SerializeField] protected float defaultWeaponAttack = 1;
    [SerializeField] protected float defaultDefence = 5;
    [SerializeField] protected float defaultMagicDefence = 0;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected Slider hpS;


    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // The order of writing and reading is really important
        // Not need to send or read position data, Other component is doing this.
        // ORDER:
        //      1. hitPoints
        //      2. currentHitPoints
        //      3. defaultAttack
        //      4. defaultWeaponAttack
        //      5. defaultDefence
        //      6. defaultMagicDefence
        //      7. speed

        if (stream.IsWriting)
        {
            stream.SendNext(hitPoints);
            stream.SendNext(currentHitPoints);
            stream.SendNext(defaultAttack);
            stream.SendNext(defaultWeaponAttack);
            stream.SendNext(defaultDefence);
            stream.SendNext(defaultMagicDefence);
            stream.SendNext(speed);
        }
        else
        {
            hitPoints = (float)stream.ReceiveNext();
            currentHitPoints = (float)stream.ReceiveNext();
            defaultAttack = (float)stream.ReceiveNext();
            defaultWeaponAttack = (float)stream.ReceiveNext();
            defaultDefence = (float)stream.ReceiveNext();
            defaultMagicDefence = (float)stream.ReceiveNext();
            speed = (float)stream.ReceiveNext();
        }
    }

    // Getters and Setters
    public float HitPoints
    {
        get { return hitPoints; }
        set { hitPoints = value; }
    }

    public float CurrentHitPoints
    {
        get { return currentHitPoints; }
        set { currentHitPoints = value; }
    }

    public float DefaultAttack
    {
        get { return defaultAttack; }
        set { defaultAttack = value; }
    }

    public float DefaultWeaponAttack
    {
        get { return defaultWeaponAttack; }
        set { defaultWeaponAttack = value; }
    }

    public float DefaultDefence
    {
        get { return defaultDefence; }
        set { defaultDefence = value; }
    }

    public float DefaultmagicDefence
    {
        get { return defaultMagicDefence; }
        set { defaultMagicDefence = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // Update
    void Update() {

    }

    // Taking Damage
    public void TakeDamage(float damage) {

        currentHitPoints -= damage;
        if (currentHitPoints > hitPoints) {
            currentHitPoints = hitPoints;
        }
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
    protected void _activate()
    {
        gameObject.SetActive(true);
    }

    // Dying
    [PunRPC]
    protected void _deactivate()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(10f, -10f, -10f);
    }
}
