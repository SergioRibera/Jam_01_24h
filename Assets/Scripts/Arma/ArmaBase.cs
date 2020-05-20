using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaBase : MonoBehaviour
{
    public string weaponName;
    public Sprite mySprite;
    public int damage;
    public bool excecuteDamage = false;
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Enemy")) 
            if (excecuteDamage)
            {
                c.GetComponent<EnemyBehaviour>().Damage(damage);
                excecuteDamage = false;
            }
    }
    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.CompareTag("Enemy"))
            if (excecuteDamage)
            {
                c.GetComponent<EnemyBehaviour>().Damage(damage);
                excecuteDamage = false;
            }
    }
    public IEnumerator EnabledNow()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Collider2D>().enabled = true;
    }
    public void ExecuteDamage() => excecuteDamage = true;
}
