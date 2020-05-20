using System;
using UnityEngine;
[System.Serializable]
public class GeneralInputs{
    Joystick j;
    InputsPc InputsPc;
    public GeneralInputs(Joystick _j)
    {
        j = _j;
        InputsPc = InputsPc.Default;
    }

    public float HorizontalAxis
    {
        get
        {
            return j.Horizontal;
        }
    }
    public float VerticalAxis
    {
        get
        {
            return j.Vertical;
        }
    }
}

[Serializable]
public class InputsPc
{
    public KeyCode avanza = KeyCode.W;
    public KeyCode retrocede = KeyCode.S;
    public KeyCode derecha = KeyCode.D;
    public KeyCode izquierda = KeyCode.A;
    public KeyCode simpleAttack = KeyCode.Mouse0;
    public KeyCode openBag = KeyCode.E;
    public KeyCode closeBag = KeyCode.E;

    public static InputsPc Default
    {
        get
        {
            InputsPc i = new InputsPc();
            i.avanza = KeyCode.W;
            i.retrocede = KeyCode.S;
            i.derecha = KeyCode.D;
            i.izquierda = KeyCode.A;
            i.simpleAttack = KeyCode.Mouse0;
            i.openBag = KeyCode.E;
            i.closeBag = KeyCode.E;
            return i;
        }
    }
}