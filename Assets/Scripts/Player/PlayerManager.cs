using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : PlayerBehaviour
{
    public Slider lifeBar;
    public TMPro.TextMeshProUGUI textWeapon;
    public Image imageWeapon;
    public ArmaBase arma = null;

    private new void Start()
    {
        base.Start();
        ManagerDataPlayer.SetListenerDown("attack", Attack);
        lifeBar.maxValue = life;
        lifeBar.value = life;
    }
    void Attack(bool press)
    {
        if (press)
        {
            AnimSetTrigger("attack");
            arma?.ExecuteDamage();
        }
    }
    public override void OnDamaged()
    {
        lifeBar.value = life;
    }
    public override void Dead()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arma"))
        {
            if (arma != null)
            {
                arma.GetComponent<Collider2D>().enabled = false;
                StartCoroutine(arma.EnabledNow());
                arma.transform.SetParent(null);
            }

            GameObject a = other.gameObject;
            a.transform.SetParent(transform.GetChild(0).GetChild(0));
            a.transform.localPosition = Vector3.zero;
            a.transform.localRotation = Quaternion.identity;
            arma = other.GetComponent<ArmaBase>();
            textWeapon.gameObject.SetActive(true);
            imageWeapon.gameObject.SetActive(true);
            textWeapon.text = arma.weaponName;
            imageWeapon.sprite = arma.mySprite;
        }
    }
}