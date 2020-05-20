using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using UnityEditor.Animations;
using UnityEngine;

public delegate void Evento();
public class PlayerBehaviour : MonoBehaviour
{
    public AnimatorController controllerAnim = null;
    public Joystick joystick;
    public bool is2D;
    public float velocidad = 5f;
    public float turnVelocity = 5f;
    public int life = 10;

    PlayerCamera2D camera2D;
    Animator anim = null;
    Rigidbody rb = null;
    Rigidbody2D rb2D = null;
    Vector3 pos;
    //internal Evento onMove;
    public void Start()
    {
        ManagerDataPlayer.Init(joystick);
        gameObject.tag = "Player";
        if (GetComponent<Animator>() != null)
            anim = GetComponent<Animator>();
        if (controllerAnim != null)
            anim.runtimeAnimatorController = controllerAnim;
        if (!is2D)
        {
            if (!GetComponent<Rigidbody>())
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            else
                rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }
        else
        {
            if (!GetComponent<Rigidbody2D>())
            {
                rb2D = gameObject.AddComponent<Rigidbody2D>();
            }
            else
                rb2D = GetComponent<Rigidbody2D>();
            rb2D.freezeRotation = true;
            camera2D = Camera.main.GetComponent<PlayerCamera2D>();
            camera2D.Init(transform);
        }
        //onMove += Move;
        //joystick.onMove += onMove;
    }
    public void Update()
    {

    }
    public void LateUpdate()
    {
        //animControl.CalculateAnim();
    }
    public void FixedUpdate()
    {
        Move();
        camera2D.MoveCam();
    }
    void Move()
    {
        float x = joystick.Horizontal;
        float z = joystick.Vertical;
        if (x != 0 || z != 0)
        {
            if (!is2D)
            {
                pos.Set(x, 0, z);
                pos = pos.normalized * velocidad * Time.deltaTime;
                rb.MovePosition(transform.position + pos);
            }
            else
            {
                pos.Set(x, z, 0);
                pos = pos.normalized * velocidad * Time.deltaTime;
                float r = (x > 0) ? -45f : (x < 0) ? 45f : 0f;
                Quaternion newrotation = Quaternion.Slerp(Quaternion.AngleAxis(rb2D.rotation+r, Vector3.forward), Quaternion.LookRotation(pos), turnVelocity * Time.deltaTime);
                rb2D.MovePosition(transform.position + pos);
                rb2D.SetRotation(newrotation);
            }
            return;
        }
    }

    public void Damage(int d)
    {
        life = (life - d) > 0 ? life - d : 0;
        if (life <= 0)
        {
            life = 0;
            Dead();
        }
        OnDamaged();
    }
    public virtual void OnDamaged()
    {

    }
    public virtual void Dead()
    {

    }

    internal void AnimSetTrigger(string name)
    {
        if (anim == null)
            if (GetComponent<Animator>())
                anim = GetComponent<Animator>();
            else
                throw new System.Exception("El jugador no tiene un player Controler");
        anim.SetTrigger(name);
    }
    internal void AnimSetBoolean(string name, bool value)
    {
        if (anim == null)
            if (GetComponent<Animator>())
                anim = GetComponent<Animator>();
            else
                throw new System.Exception("El jugador no tiene un player Controler");
        anim.SetBool(name, value);
    }
    internal void AnimSetFloat(string name, float value)
    {
        if (anim == null)
            if (GetComponent<Animator>())
                anim = GetComponent<Animator>();
            else
                throw new System.Exception("El jugador no tiene un player Controler");
        anim.SetFloat(name, value);
    }
    internal void AnimSetInteger(string name, int value)
    {
        if (anim == null)
            if (GetComponent<Animator>())
                anim = GetComponent<Animator>();
            else
                throw new System.Exception("El jugador no tiene un player Controler");
        anim.SetInteger(name, value);
    }
    internal void Wait(int seconds) => Thread.Sleep(seconds * 1000);
}