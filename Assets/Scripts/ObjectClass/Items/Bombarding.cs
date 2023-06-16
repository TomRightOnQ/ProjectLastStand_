using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

// Bombarding with preview
[RequireComponent(typeof(PhotonView))]
public class Bombarding : Items
{
    [SerializeField] Slider previewS;
    [SerializeField] private float time = 1.5f;
    private const string PREFAB_LOC = "Prefabs/";
    void Awake()
    {
        gameObject.tag = "Proj";
        SetUp(100, -1, time);
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(damage);
            stream.SendNext(owner);
            stream.SendNext(damageRange);
            stream.SendNext(hitAnim);
            stream.SendNext(isMagic);
            stream.SendNext(time);
        }
        else
        {
            damage = (float)stream.ReceiveNext();
            owner = (int)stream.ReceiveNext();
            damageRange = (float)stream.ReceiveNext();
            hitAnim = (int)stream.ReceiveNext();
            isMagic = (bool)stream.ReceiveNext();
            time = (float)stream.ReceiveNext();
        }
    }

    public void SetUp(int id, int _owner, float _time)
    {
        WeaponConfigs.WeaponConfig weaponConfig = WeaponConfigs.Instance._getWeaponConfig(id);
        owner = _owner;
        damage = weaponConfig.attack;
        hitAnim = weaponConfig.hitAnim;
        damageRange = weaponConfig.damageRange;
        isMagic = weaponConfig.isMagic;
        time = _time;
        StartCoroutine(TriggerExplosion());
        StartCoroutine(FillSlider());
    }

    public IEnumerator TriggerExplosion()
    {
        yield return new WaitForSeconds(time);

        explode();
        Destroy(this.gameObject);
    }

    public IEnumerator FillSlider()
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            float fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / time);
            previewS.value = fillAmount;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        previewS.value = 1f;
    }

    private void explode()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z - 1.5f);
        AnimManager.Instance.PlayAnim(0, pos, new Vector3(damageRange, damageRange, damageRange));
        AudioManager.Instance.PlaySound(9, transform.position);
        if (PhotonNetwork.IsConnected)
        {
            Explosions explosion = PhotonNetwork.Instantiate(PREFAB_LOC + PrefabManager.Instance.ExplosionPrefab.name, transform.position, Quaternion.identity).GetComponent<Explosions>();
            explosion.Initialize(damageRange * 1.75f, damage, 0, isMagic, owner, hitAnim);
        }
        else
        {
            Explosions explosion = Instantiate(PrefabManager.Instance.ExplosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosions>();
            explosion.Initialize(damageRange * 1.75f, damage, 0, isMagic, owner, hitAnim);
        }
    }
}
