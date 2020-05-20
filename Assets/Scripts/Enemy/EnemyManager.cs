using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : EnemyBehaviour
{
    public float velocidad = 5f;
    public float turnVelocity = 1.5f;
    public float minDistance = 30f;
    public float offsetDistance = 4f;

    public int damage = 1;

    Animator anim;
    Rigidbody2D rb;
    Transform player;
    public bool goToPlayer;
    public bool isDestination;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        onDead += Dead;
    }
    private void LateUpdate() {
        if (player == null) return;
        goToPlayer = (Vector2.Distance(transform.position, player.position) - offsetDistance) <= minDistance;
        isDestination = (Vector2.Distance(transform.position, player.position) - offsetDistance) <= 0;
        anim.SetBool("move", goToPlayer && !isDestination);
    }
    private void FixedUpdate()
    {
        if (player == null) return;
        if (goToPlayer && !isDestination)
        {
            Quaternion newrotation = Quaternion.Slerp(Quaternion.AngleAxis(rb.rotation, Vector3.forward), Quaternion.LookRotation(player.position), turnVelocity * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, player.position, 5 * Time.deltaTime);
            rb.SetRotation(newrotation);
        }
        anim.SetBool("attack", isDestination);
    }
    public new void Dead() { 
        player = null;
    }
    void Attack()
    {
        if(isDestination && player != null)
        {
            player.GetComponent<PlayerBehaviour>().Damage(damage);
        }
    }
}