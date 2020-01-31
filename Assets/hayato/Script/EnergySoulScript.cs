using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySoulScript : MonoBehaviour {

    private bool isMaxCharge;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _teslaHitEffect; // テスラキャノンが敵に当たったときに出てくるエフェクト
    [SerializeField] private GameObject _smallEffect;
    private GameObject instantTeslaHitEffect;   // Instantiateされたテスラキャノン着弾エフェクト
    private float damage;

    LastBossScript lastBossScript;

    void Start () {
        lastBossScript = GameObject.Find("LastBoss").GetComponent<LastBossScript>();
	}
	
    private void FixedUpdate()
    {
        transform.Translate(new Vector3(_speed, 0, 0));
    }

    public void SetParameter(bool maxCharge, Color chargeColor, float teslaChargeTime)
    {
        isMaxCharge = maxCharge;
        GetComponent<SpriteRenderer>().color = chargeColor;
        damage = teslaChargeTime;
    }

    private IEnumerator UltraTeslaCanon()
    {
        yield return new WaitForSeconds(0.75f);
        instantTeslaHitEffect = GameObject.Instantiate(_teslaHitEffect) as GameObject;
        Destroy(instantTeslaHitEffect, 3.5f);
        SoundManager.Instance.PlaySeByName("cannon2");
        SoundManager.Instance.PlaySeByName("beamgun2");
        SoundManager.Instance.PlaySeByName("cannon2");
        SoundManager.Instance.PlaySeByName("beamgun2");
        lastBossScript.BossHPDamage(100);
        yield return new WaitForSeconds(0.75f);
        SoundManager.Instance.PlaySeByName("magic-gravity2");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "LastBoss") {
            if(isMaxCharge) {
                StartCoroutine("UltraTeslaCanon");
                GetComponent<SpriteRenderer>().enabled = false;
            } else {
                SoundManager.Instance.PlaySeByName("bomb1");
                lastBossScript.BossHPDamage(damage);
                GameObject instantTeslaHitEffect = GameObject.Instantiate(_smallEffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(instantTeslaHitEffect, 3.5f);
                Destroy(gameObject);
            }
            
        }
    }
}
