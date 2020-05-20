using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int life = 5;
    internal event Evento onDead;
    public void Damage(int d)
    {
        life = (life - d) > 0 ? life - d : 0;
        if (life <= 0)
        {
            life = 0;
            Dead();
        }
    }
    public void Dead()
    {
        onDead?.Invoke();
        enabled = false;
        GetComponent<Animator>().SetTrigger("dead");
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 5.0f);
    }
}